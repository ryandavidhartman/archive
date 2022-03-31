package course.labs.locationlab;

import java.util.ArrayList;
import java.util.List;

import android.app.ListActivity;
import android.content.Context;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

public class PlaceViewActivity extends ListActivity implements LocationListener {
	private static final long FIVE_MINS = 5 * 60 * 1000;

	private static String TAG = "Lab-Location";

	private Location mLastLocationReading;
	private PlaceViewAdapter mAdapter;

	// default minimum time between new readings
	private long mMinTime = 5000;

	// default minimum distance between old and new readings.
	private float mMinDistance = 1000.0f;

	private LocationManager mLocationManager;

	// A fake location provider used for testing
	private MockLocationProvider mMockLocationProvider;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

        mAdapter = new PlaceViewAdapter(this);

        ListView listView = getListView();

        listView.setFooterDividersEnabled(true);
        TextView footerView = (TextView) getLayoutInflater().inflate(R.layout.footer_view, null);

        if (null == (mLocationManager = (LocationManager) getSystemService(Context.LOCATION_SERVICE)))
            finish();

        footerView.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {

                log("Entered footerView.OnClickListener.onClick()");
                mLastLocationReading = bestLastKnownLocation(mMinDistance, mMinTime);

                if (mLastLocationReading == null){
                    onNoCurrentLocation(v);
                }
                else if (mAdapter.intersects(mLastLocationReading)){
                    onLocationHasBeenSeen();
                }
                else{
                    onNewCurrentLocation(mLastLocationReading);
                }
            }
        });

        listView.addFooterView(footerView);
        listView.setAdapter(mAdapter);
	}

    private void onNewCurrentLocation(Location location) {
        log("Starting Place Download");
        downloadNewPlaceBadge(location);
    }

    private void onLocationHasBeenSeen() {

        String text = "You already have this location badge";

        log(text);
        Toast.makeText(getApplicationContext(), text, Toast.LENGTH_LONG);
    }

    private void onNoCurrentLocation(View v) {
        log("Location data is not available");
        Toast.makeText(getApplicationContext(), "Location data is not available", Toast.LENGTH_LONG);

//        v.setEnabled(false);
    }

    // Get the last known location from all providers
    // return best reading that is as accurate as minAccuracy and
    // was taken no longer then minAge milliseconds ago. If none,
    // return null.

    private Location bestLastKnownLocation(float minAccuracy, long maxAge) {

        Location bestResult = null;
        float bestAccuracy = Float.MAX_VALUE;
        long bestAge = Long.MIN_VALUE;

        List<String> matchingProviders = mLocationManager.getAllProviders();

        for (String provider : matchingProviders) {

            Location location = mLocationManager.getLastKnownLocation(provider);

            if (location != null) {

                float accuracy = location.getAccuracy();
                long time = location.getTime();

                if (accuracy < bestAccuracy) {

                    bestResult = location;
                    bestAccuracy = accuracy;
                    bestAge = time;

                }
            }
        }

        // Return best reading or null
        if (bestAccuracy > minAccuracy || (System.currentTimeMillis() - bestAge) > maxAge) {
            return null;
        } else {
            return bestResult;
        }
    }

    private void downloadNewPlaceBadge(Location location) {
        new PlaceDownloaderTask(this).execute(location);
    }

	@Override
	protected void onResume() {
		super.onResume();

		mMockLocationProvider = new MockLocationProvider(
				LocationManager.NETWORK_PROVIDER, this);

        // TODO - Check NETWORK_PROVIDER for an existing location reading.
        // Only keep this last reading if it is fresh - less than 5 minutes old.

	
		
        // TODO - register to receive location updates from NETWORK_PROVIDER


		
	}

	@Override
	protected void onPause() {

		mMockLocationProvider.shutdown();

		// TODO - unregister for location updates


		
		super.onPause();
	}

	// Callback method used by PlaceDownloaderTask
	public void addNewPlace(PlaceRecord place) {

		log("Entered addNewPlace()");
		mAdapter.add(place);

	}

	@Override
	public void onLocationChanged(Location currentLocation) {

        // TODO - Handle location updates
        // Cases to consider
        // 1) If there is no last location, keep the current location.
        // 2) If the current location is older than the last location, ignore
        // the current location
        // 3) If the current location is newer than the last locations, keep the
        // current location.


	}

	@Override
	public void onProviderDisabled(String provider) {
		// not implemented
	}

	@Override
	public void onProviderEnabled(String provider) {
		// not implemented
	}

	@Override
	public void onStatusChanged(String provider, int status, Bundle extras) {
		// not implemented
	}

	private long age(Location location) {
		return System.currentTimeMillis() - location.getTime();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		MenuInflater inflater = getMenuInflater();
		inflater.inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		switch (item.getItemId()) {
		case R.id.print_badges:
			ArrayList<PlaceRecord> currData = mAdapter.getList();
			for (int i = 0; i < currData.size(); i++) {
				log(currData.get(i).toString());
			}
			return true;
		case R.id.delete_badges:
			mAdapter.removeAllViews();
			return true;
		case R.id.place_one:
			mMockLocationProvider.pushLocation(37.422, -122.084);
			return true;
		case R.id.place_invalid:
			mMockLocationProvider.pushLocation(0, 0);
			return true;
		case R.id.place_two:
			mMockLocationProvider.pushLocation(38.996667, -76.9275);
			return true;
		default:
			return super.onOptionsItemSelected(item);
		}
	}

	private static void log(String msg) {
		try {
			Thread.sleep(500);
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
		Log.i(TAG, msg);
	}

}
