import http.server
import os
import sys


if __name__ == "__main__":

    sys.stdout = sys.stderr = open(os.devnull, "w")

    httpd = http.server.HTTPServer(("localhost", 8000), http.server.SimpleHTTPRequestHandler)
    httpd.serve_forever()