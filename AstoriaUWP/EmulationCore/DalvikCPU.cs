// DalvikCPU

using AndroidInteropLib;
using AndroidInteropLib.android.content;
using AndroidInteropLib.android.view;
using DalvikUWPCSharp.Applet;
using DalvikUWPCSharp.Reassembly;
using dex.net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// DalvikUWPCSharp.Classes
namespace DalvikUWPCSharp.Classes
{

    // DalvikCPU class
    public class DalvikCPU
    {
        //List<object> Registers = new List<object>();
        object[] Registers = new object[16];
        object result;
        public Dex dex;
        string packageName;
        public EmuPage hostPage;
        DroidApp da;
        //int LastRegisterModified;

        private Context appContext;
        private Window droidWindow;


        // DalvikCPU(dex, pName, host emupage)
        public DalvikCPU(Dex d, string pName, EmuPage hostPg)
        {
            dex = d;
            packageName = pName;
            hostPage = hostPg;
            da = hostPage.RunningApp;
            da.cpu = this;

            // set preload status "Setting up app environment"
            hostPage.setPreloadStatusText("Setting up app environment...");

        }//DalvikCPU end

        // Start 
        public async void Start()
        {
            if (appContext == null)
            {
                appContext = new AstoriaContext(da, await AstoriaResources.CreateAsync(da));
                
                // form Astoria Window
                droidWindow = new AstoriaWindow(appContext, hostPage);
            }

            // for each dex classes...
            foreach(Class cl in dex.GetClasses())
            {
                //...check if package name contains MainActivity
                if(cl.Name.Equals(packageName + ".MainActivity"))
                {
                    // foreach methods...
                    foreach(Method m in cl.GetMethods())
                    {
                        //...check if method's name contains onCreate
                        if(m.Name.Equals("onCreate"))
                        {
                            // run method m of class cl
                            RunMethod(m, cl);
                        }
                    }
                }
            }

            hostPage.preloadDone();

        }//Start end


        // GoBack event handler
        public async void GoBack()
        {
            var dialog = new Windows.UI.Popups.MessageDialog("Back event initiated.", "Dalvik CPU");

            await dialog.ShowAsync();

        }// GoBack end


        // RunMethod (m, c, obj)
        public object RunMethod(Method m, Class cl, params object[] obj)
        {
            if(!TryNativeMethod(m, cl, obj))
            {
                foreach (OpCode o in m.GetInstructions())
                {
                    ExecuteInstruction(o, cl);
                }
            }

            return result;
            //dynamic MyD = new DynamicObject()

        }//RunMethod end


        // ExecuteInstruction (operand, code)
        public void ExecuteInstruction(OpCode op, Class cl)
        {
            var opType = op.Instruction;

            switch(op.Instruction)
            {
                case Instructions.Const:
                    ConstOpCode ConstOP = (ConstOpCode)op;
                    Registers[ConstOP.Destination] = ConstOP.Value;
                    break;

                case Instructions.InvokeSuper:
                    InvokeSuperOpCode op2 = (InvokeSuperOpCode)op;

                    try
                    {
                        RunMethod(dex.GetMethod(op2.MethodIndex), cl);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("[ex] Dalvik CPU Instructions.InvokeSuper Exception : "
                            + ex.Message);
                    }
                    break;

                case Instructions.InvokeVirtual:
                    InvokeVirtualOpCode ivop = (InvokeVirtualOpCode)op;

                    try
                    {
                        RunMethod(dex.GetMethod(ivop.MethodIndex), 
                            cl, 
                            Registers[ivop.ArgumentRegisters[1]]);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("[ex] Dalvik CPU Instructions.InvokeVirtual Exception : " 
                            + ex.Message);
                    }
                    break;

                case Instructions.MoveResult:
                    MoveResultOpCode movR = (MoveResultOpCode)op;
                    Registers[movR.Destination] = result;
                    break;

                case Instructions.ReturnVoid:
                    //Clear result
                    result = null;
                    break;

                default:
                    Debug.WriteLine("Unhandled Instruction");
                    break;
            }

            // DEBUG 
            //Debug.WriteLine("Executed: " + op.ToString());

        }//ExecuteInstruction end


        // TryNativeMethod (m, c, obj)
        private bool TryNativeMethod(Method m, Class c, params object[] obj)
        {
            // if method name contains "setContentView"..
            if (m.Name.Contains("setContentView"))
            {
                // ...set contentview
                droidWindow.setContentView((int)obj[0]);

                return true;
            }

            string className = ConvertClassName(c.Name);
            if (className.StartsWith(packageName))
                return false;
            
            Type myType = Type.GetType("AndroidInteropLib." + className);
            if(myType != null)
            {
                TypeInfo info = myType.GetTypeInfo();
                MethodInfo mi = info.GetDeclaredMethod(m.Name);
                if (mi != null)
                {
                    try
                    {
                        mi.Invoke(this, obj);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }  
                }
            }

            return false;

        }//TryNativeMethod end


        // ConvertClassName
        private string ConvertClassName(string s)
        {
            return s.Replace("internal", "_internal");

        }//ConvertClassName end

    }//DalvikCPU class end


    /*
    public class DalvikClass
    {
        Type super;
        //object super;
        Class c;
        DalvikCPU cpu;

        public DalvikClass(Class c, DalvikCPU dc)
        {
            this.c = c;
            cpu = dc;

            if("AndroidInteropLib" + c.SuperClass == "")
            {
                //set super to native class
            }
        }

        public void SetInheritence(Type t)
        {
            super = t;
        }

        public object RunMethod(string name, params object[] obj)
        {
            //Check if current class has method. If not, check super.
            var meth = c.GetMethods().FirstOrDefault(x => x.Name.Equals(name));
            if (meth != null)
                return cpu.RunMethod(meth, c);

            if (super != null)
            {
                TypeInfo info = super.GetTypeInfo();
                MethodInfo mi = info.GetDeclaredMethod(name);
                if (mi != null)
                {
                    try
                    {
                        return mi.Invoke(this, obj);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        // GetSuperType
        private Type GetSuperType()
        {
            return super;
        }

    }//DalvikClass end
    */

}//DalvikUWPCSharp.Classes namespace end
