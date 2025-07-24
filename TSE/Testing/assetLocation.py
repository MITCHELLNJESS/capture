from geopy.distance import distance, geodesic
from geopy import Point
import math

def geodetic_to_unreal(lat, lon, origin_lat, origin_lon):
    # WGS-84 constants for flat approximation
    lat_m_per_deg = 111320.0
    lon_m_per_deg = math.cos(math.radians(origin_lat)) * 111320.0

    # Delta in degrees
    delta_lat = lat - origin_lat
    delta_lon = lon - origin_lon

    # Convert to meters
    delta_x_m = delta_lat * lat_m_per_deg
    delta_y_m = delta_lon * lon_m_per_deg

    # Convert to Unreal units (cm)
    unreal_x = -1045.89 - (delta_x_m * 100.0)  # East
    unreal_y = 1041.62 - (delta_y_m * 100.0)  # North

    return unreal_x, unreal_y

# Starting point (latitude, longitude)
start_lat = 38.750715246632836
start_lon = -77.49721299979501

# Distance in meters (radius of demo area)
dist_m = 21.8

# Bearing in degrees (azimuth: 0° is north, 90° is east, etc.)
# bearings = [60, 90, 120, 150, 180, 210]
bearings = [30, 0, 330, 300, 270, 240]
assets = []

# Calculate destination points
for bearing in bearings:
    destination = distance(meters=dist_m).destination(point=Point(start_lat, start_lon), bearing=bearing)
    assets.append(destination)

for asset in assets:
    unreal_x, unreal_y = geodetic_to_unreal(start_lat, start_lon, asset.latitude, asset.longitude)
    print(f"Geodetic: ({asset.latitude}, {asset.longitude}) Unreal: ({unreal_x}, {unreal_y})")