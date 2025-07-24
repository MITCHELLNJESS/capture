import math
from geopy.distance import distance
from geopy import Point

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
    unreal_x = delta_x_m * 100.0  # East
    unreal_y = delta_y_m * 100.0  # North

    return unreal_x, unreal_y

# Example usage
origin_lat = 38.75080920
origin_lon = -77.49733298

dest_lat = 38.750715246632836
dest_lon = -77.49721299979501

x, y = geodetic_to_unreal(dest_lat, dest_lon, origin_lat, origin_lon)
print(f"Unreal Coordinates (cm): X={x:.2f}, Y={y:.2f}")

center = distance(meters=14.75).destination(point=Point(38.75080920, -77.49733298), bearing=135)
print(f"{center.latitude}, {center.longitude}")