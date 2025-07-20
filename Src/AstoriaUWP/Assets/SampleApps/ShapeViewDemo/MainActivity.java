package com.example.shapeviewdemo;

import android.app.Activity;
import android.os.Bundle;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

// This is a sample Android app that would create a white square on a black background
// In AstoriaUWP, this would be rendered using our custom ShapeView
public class MainActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        // Option 1: Load from XML layout
        setContentView(R.layout.activity_main);
        
        /* Option 2: Create programmatically
        // Create a RelativeLayout as the root container
        RelativeLayout rootLayout = new RelativeLayout(this);
        rootLayout.setLayoutParams(new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MATCH_PARENT,
                ViewGroup.LayoutParams.MATCH_PARENT));
        
        // Create a ShapeView (in a real Android app, this would be a custom View)
        ShapeView shapeView = new ShapeView(this, null);
        
        // Set layout parameters for the ShapeView
        RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(200, 200);
        params.addRule(RelativeLayout.CENTER_IN_PARENT);
        shapeView.setLayoutParams(params);
        
        // Add the ShapeView to the root layout
        rootLayout.addView(shapeView);
        
        // Set the root layout as the content view
        setContentView(rootLayout);
        */
    }
}