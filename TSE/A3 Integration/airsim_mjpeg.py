import airsim
import cv2
from flask import Flask, Response
import queue
import numpy as np
import threading

app = Flask(__name__)

client = airsim.MultirotorClient()
client.confirmConnection()

frame_queue = queue.Queue(maxsize=1)

def fetch_frames():
    while True:
        try:
            raw_image = client.simGetImage("a3", airsim.ImageType.Scene)
            if raw_image is None:
                continue
            jpg = np.frombuffer(raw_image, dtype=np.uint8)
            frame = cv2.imdecode(jpg, cv2.IMREAD_COLOR)
            if frame is None:
                continue
            frame = cv2.resize(frame, (640, 480))
            ret, jpeg = cv2.imencode('.jpg', frame)
            if ret:
                if not frame_queue.full():
                    frame_queue.put(jpeg.tobytes())
        except Exception as e:
            print(f"Frame fetch error: {e}")

def generate_mjpeg():
    while True:
        frame = frame_queue.get()
        yield (b'--frame\r\n'
               b'Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n')

@app.route('/video_feed')
def video_feed():
    return Response(generate_mjpeg(),
                    mimetype='multipart/x-mixed-replace; boundary=frame')

if __name__ == '__main__':
    threading.Thread(target=fetch_frames, daemon=True).start()
    app.run(host='0.0.0.0', port=5000, threaded=True, debug=False, use_reloader=False)