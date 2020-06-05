# File uploads service ![build status ](https://travis-ci.org/alexkuznetsov/FileUploaderApp.svg?branch=dev)

REST upload file service.

Service can:

 - Upload files to the store (currently, only filesystem, but it pluggable and can be replaced with DB, S3 or something);
 
 - Download files by it identifier.

For launching (based on image on docker hub):

Open console (e.g. linux shell) and `cd` to user's folder, ``` cd ~/ ```

```
mkdir fileuploaderapp && cd fileuploaderapp 
mkdir logs
mkdir uploads
mkdir nginx
wget -P nginx https://raw.githubusercontent.com/alexkuznetsov/FileUploaderApp/dev/nginx/nginx.conf
wget https://raw.githubusercontent.com/alexkuznetsov/FileUploaderApp/dev/docker-compose.yml
docker-compose up
```

After that, Docker retrives the last version of the container and launches it.

Build image locally
===================

```
git clone https://github.com/alexkuznetsov/FileUploaderApp.git
cd FileUploaderApp
git checkout dev
mkdir logs
mkdir uploads
docker-compose -f docker-compose-local.yml up
```

Endpoints
===========

1.  ```/api/token```

    Authentication endpoint, checks username/password pair and returns JWT for upload endpoint.

    Sample request: 
```
{
    "username": "rex", 
    "password": "passw" 
} 
```

2. ```/api/upload```
    Upload endpoint. Can consume:
    
    - Multipart Form Data;
    - JSON, with base64 encoded file ( ``` { "files": [  { "file": "some-file-name", "data": "base64 encoded data" }   ] }  ``` );
    - JSON, with links for download files (  ```{ "links": [ "http://localhost/1.bmp", "http://localhost/2.bmp" ] }``` );
    - Mix of base64 encoded files and links ( ```{ "files": [ ... ], "links": [ ... ] }``` ).

3. ```/api/file/{fileId}```

    Download file endpoint

4. ```/health```
    
    Health check endpoint

Configuration
===============

Maximum request body size (e.g. total upload size limit) can be changed by setting FILEUPLOADERAPP_LIMIT environment variable.
It can accept two types of values - string and long. String - just `NO`, when turns off maximum request body size check, and you can upload any size.
If it is a long, it is the body size in bytes, the maximum size that can be loaded.

Have questions?
===============
If you have a question - feel free to fill an issue, and i will answer you shortly as i can.

