# ShapeView Demo for AstoriaUWP

This sample demonstrates how to render a simple white square on a black background in AstoriaUWP, which is a common graphical primitive in Android applications.

## Implementation Details

### In AstoriaUWP

In AstoriaUWP, we've implemented a custom `ShapeView` class that uses Windows UI XAML controls to render a white square on a black background. The implementation consists of:

1. **ShapeView.cs**: A custom View class that creates a Rectangle XAML control with a white fill on a black background.
   - Located at: `c:\Users\Admin\source\repos\!Trae\AstoriaUWP\Src\AndroidUILib\ticomware\interop\ShapeView.cs`

2. **Renderer.cs**: Modified to handle the ShapeView when rendering XML layouts.
   - Located at: `c:\Users\Admin\source\repos\!Trae\AstoriaUWP\Src\AstoriaUWP\Reassembly\UI\Renderer.cs`

### Sample Usage

#### XML Layout

You can use the ShapeView in an Android XML layout like this:

```xml
<RelativeLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="match_parent">

    <ShapeView
        android:layout_width="200"
        android:layout_height="200"
        android:layout_centerInParent="true" />

</RelativeLayout>
```

#### Programmatic Creation

You can also create a ShapeView programmatically in your Android code:

```java
// Create a ShapeView
ShapeView shapeView = new ShapeView(context, null);

// Set layout parameters
RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(200, 200);
params.addRule(RelativeLayout.CENTER_IN_PARENT);
shapeView.setLayoutParams(params);

// Add to a container
container.addView(shapeView);
```

## How It Works

### Android Implementation

In a real Android app, a custom View that draws a white square would typically:

1. Override the `onDraw(Canvas canvas)` method
2. Use the Canvas to draw a rectangle with a white Paint
3. Set the background color to black

See `ShapeView.java` for a complete Android implementation.

### AstoriaUWP Implementation

In AstoriaUWP, we:

1. Create a custom View class that extends the base View class
2. Override the CreateWinUI method to create a Rectangle XAML control
3. Set the Rectangle's Fill to white and the container's Background to black
4. Modify the Renderer to handle our custom ShapeView when rendering XML layouts

## Resource Handling in AstoriaUWP

AstoriaUWP handles Android resources (strings, drawables, etc.) by:

1. Extracting resources from the APK file
2. Parsing the resource files (strings.xml, drawables, etc.)
3. Converting Android resources to equivalent Windows UI XAML resources
4. Using these resources when rendering the UI

For graphical primitives like shapes, AstoriaUWP uses Windows UI XAML controls (Rectangle, Ellipse, etc.) to render them, rather than implementing a full Canvas-based drawing system like Android.