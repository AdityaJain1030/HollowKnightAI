import websockets
import asyncio

import numpy as np
# import matplotlib.pyplot as plt
import cv2

# Server data
PORT = 8080
print("Server listening on Port " + str(PORT))
# A set of connected ws clients
connected = set()

# The main behavior function for this server
async def echo(websocket, path):
    print("New connection")
    while True:
        data = await websocket.recv()
        data = np.frombuffer(data, dtype=np.uint8)
        data = data.reshape(1280, 720)
        data = np.rot90(data)
        # np.flip(data)
        # np.flip(data, axis=0)
        data = np.expand_dims(data, axis=2)
        # np.flip(data)
        # im = cv2.imread
        cv2.imshow("test", data)
        cv2.waitKey(1)



    # print(data)
    # int_values = [x for x in data]
    # print(int_values)
    # print(data)
    # data = np.frombuffer(data, dtype=np.uint8)
    # data = data.reshape((212, 120))
    # plt.imshow(data)
    # plt.show()

async def main():
    async with websockets.serve(echo, "localhost", 8080):
        await asyncio.Future()  # run forever

if __name__ == "__main__":
    asyncio.run(main())