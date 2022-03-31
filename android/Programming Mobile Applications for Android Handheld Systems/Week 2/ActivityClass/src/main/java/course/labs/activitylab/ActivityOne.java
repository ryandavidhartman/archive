package course.labs.activitylab;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

public class ActivityOne extends Activity {

    private static final String RESTART_KEY = "restart";
	private static final String RESUME_KEY = "resume";
	private static final String START_KEY = "start";
	private static final String CREATE_KEY = "create";

	// String for LogCat documentation
    public static final String TAG = "Lab-ActivityOne";

    private int mCreate;
    private int mStart;
    private int mResume;
    private int mRestart;

    private TextView mTvCreate;
    private TextView mTvStart;
    private TextView mTvResume;
    private TextView mTvRestart;

    @Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_one);

        mTvCreate = (TextView) findViewById(R.id.create);
        mTvStart = (TextView) findViewById(R.id.start);
        mTvResume = (TextView) findViewById(R.id.resume);
        mTvRestart = (TextView) findViewById(R.id.restart);

        Button launchActivityTwoButton = (Button) findViewById(R.id.bLaunchActivityTwo);
		launchActivityTwoButton.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
                startActivity(new Intent(v.getContext(), ActivityTwo.class));
			}
		});

        // Check for previously saved state
        if (savedInstanceState != null) {
            mCreate = savedInstanceState.getInt(CREATE_KEY);
            mStart = savedInstanceState.getInt(START_KEY);
            mResume = savedInstanceState.getInt(RESUME_KEY);
            mRestart = savedInstanceState.getInt(RESTART_KEY);
        }

        Log.i(TAG, "Entered the onCreate() method");

        mCreate++;
        displayCounts();
	}

    @Override
    public void onStart() {
        super.onStart();

        Log.i(TAG, "Entered the onStart() method");

        mStart++;
        displayCounts();
    }

    @Override
    public void onResume() {
        super.onResume();

        Log.i(TAG, "Entered the onResume() method");

        mResume++;
        displayCounts();
    }

    @Override
    public void onPause() {
        super.onPause();
        Log.i(TAG, "Entered the onPause() method");
    }

    @Override
    public void onStop() {
        super.onStop();
        Log.i(TAG, "Entered the onStop() method");
    }

    @Override
    public void onRestart() {
        super.onRestart();

        Log.i(TAG, "Entered the onRestart() method");

        mRestart++;
        displayCounts();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        Log.i(TAG, "Entered the onDestroy() method");
    }

    @Override
    public void onSaveInstanceState(Bundle savedInstanceState) {
        savedInstanceState.putInt(CREATE_KEY, mCreate);
        savedInstanceState.putInt(START_KEY, mStart);
        savedInstanceState.putInt(RESUME_KEY, mResume);
        savedInstanceState.putInt(RESTART_KEY, mRestart);
    }
	
	// Updates the displayed counters
	public void displayCounts() {

		mTvCreate.setText("onCreate() calls: " + mCreate);
		mTvStart.setText("onStart() calls: " + mStart);
		mTvResume.setText("onResume() calls: " + mResume);
		mTvRestart.setText("onRestart() calls: " + mRestart);
	
	}
}
