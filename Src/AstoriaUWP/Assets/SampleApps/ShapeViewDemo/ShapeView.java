package com.example.shapeviewdemo;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.util.AttributeSet;
import android.view.View;

/**
 * A custom View that draws a white square on a black background.
 * This is how it would be implemented in a real Android app.
 * In AstoriaUWP, we've created a corresponding ShapeView class that uses Windows UI XAML controls.
 */
public class ShapeView extends View {
    private Paint squarePaint;
    private Paint backgroundPaint;

    public ShapeView(Context context) {
        super(context);
        init();
    }

    public ShapeView(Context context, AttributeSet attrs) {
        super(context, attrs);
        init();
    }

    public ShapeView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        init();
    }

    private void init() {
        // Initialize the paint for the white square
        squarePaint = new Paint();
        squarePaint.setColor(Color.WHITE);
        squarePaint.setStyle(Paint.Style.FILL);

        // Initialize the paint for the black background
        backgroundPaint = new Paint();
        backgroundPaint.setColor(Color.BLACK);
        backgroundPaint.setStyle(Paint.Style.FILL);
    }

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);

        // Draw the black background
        canvas.drawRect(0, 0, getWidth(), getHeight(), backgroundPaint);

        // Calculate the square dimensions (centered in the view)
        float squareSize = Math.min(getWidth(), getHeight()) * 0.8f; // 80% of the smallest dimension
        float left = (getWidth() - squareSize) / 2;
        float top = (getHeight() - squareSize) / 2;
        float right = left + squareSize;
        float bottom = top + squareSize;

        // Draw the white square
        canvas.drawRect(left, top, right, bottom, squarePaint);
    }
}