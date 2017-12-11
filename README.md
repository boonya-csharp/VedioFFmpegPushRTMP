
## The project is going to solve what kind of problems?

In c# with FFmpeg to record camera vedio and push its file stream to RTMP Server and we use VLC media player to play the network-stream.

![Live-Vedio](https://github.com/BoonyaCSharp-ASP/VedioFFmpegPushRTMP/raw/master/SampleApp/Images/a.png)



## Push a file to RTMP Server
```
Network.Create()
	.WithSource(inputPath)
	.WithDest("rtmp://192.168.10.12/live/stream")
	.WithFilter(new X264Filter{ConstantQuantizer = 20})
	.WithFilter(new ResizeFilter(980,550))
	.Push();
```


### FFmpegLib
if you want build this project,
please donwload ffmpeg lib first.

for x32 build with:
http://ffmpeg.zeranoe.com/builds/win32/shared/ffmpeg-20141117-git-3f07dd6-win32-shared.7z

for x64 build withd:
http://ffmpeg.zeranoe.com/builds/win64/shared/ffmpeg-20141117-git-3f07dd6-win64-shared.7z

after extract the files, copy the contents of the 'bin' folder to the path '/external/ffmpeg/x32(or x64)/'


### By owner License

[License by ower](https://github.com/at0717/FFmpegSharp/blob/master/LICENSE)


## Nginx-RTMP server configuration
nginx.conf
```
worker_processes  1;

#error_log  logs/error.log;
#error_log  logs/error.log  notice;
#error_log  logs/error.log  info;

#pid        logs/nginx.pid;


events {
    worker_connections  1024;
}

rtmp {

    server {

        listen 1935;  #监听的端口

        chunk_size 4000;


        application video {
           play /usr/local/data/video;
        }

        application live{ #第一处添加的直播字段
           live on;
        }
    }
}


http {
    include       mime.types;
    default_type  application/octet-stream;

    #log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
    #                  '$status $body_bytes_sent "$http_referer" '
    #                  '"$http_user_agent" "$http_x_forwarded_for"';

    #access_log  logs/access.log  main;

    sendfile        on;
    #tcp_nopush     on;

    #keepalive_timeout  0;
    keepalive_timeout  65;

    #gzip  on;

    server {
        listen       1990;
        server_name  172.16.20.10;

        #charset koi8-r;

        #access_log  logs/host.access.log  main;

        location / {
            root   /usr/share/nginx/html;
            index  index.html index.htm;
        }

        location /stat {     #第二处添加的location字段。
            rtmp_stat all;
            rtmp_stat_stylesheet stat.xsl;
        }

       location /stat.xsl { #第二处添加的location字段。
          root /usr/local/nginx-rtmp-module/;
       }

        #error_page  404              /404.html;

        # redirect server error pages to the static page /50x.html
        #
        error_page   500 502 503 504  /50x.html;
        location = /50x.html {
            root   html;
        }

        # proxy the PHP scripts to Apache listening on 127.0.0.1:80
        #
        #location ~ \.php$ {
        #    proxy_pass   http://127.0.0.1;
        #}

        # pass the PHP scripts to FastCGI server listening on 127.0.0.1:9000
        #
        #location ~ \.php$ {
        #    root           html;
        #    fastcgi_pass   127.0.0.1:9000;
        #    fastcgi_index  index.php;
        #    fastcgi_param  SCRIPT_FILENAME  /scripts$fastcgi_script_name;
        #    include        fastcgi_params;
        #}

        # deny access to .htaccess files, if Apache's document root
        # concurs with nginx's one
        #
        #location ~ /\.ht {
        #    deny  all;
        #}
    }


    # another virtual host using mix of IP-, name-, and port-based configuration
    #
    #server {
    #    listen       8000;
    #    listen       somename:8080;
    #    server_name  somename  alias  another.alias;

    #    location / {
    #        root   html;
    #        index  index.html index.htm;
    #    }
    #}


    # HTTPS server
    #
    #server {
    #    listen       443 ssl;
    #    server_name  localhost;

    #    ssl_certificate      cert.pem;
    #    ssl_certificate_key  cert.key;

    #    ssl_session_cache    shared:SSL:1m;
    #    ssl_session_timeout  5m;

    #    ssl_ciphers  HIGH:!aNULL:!MD5;
    #    ssl_prefer_server_ciphers  on;

    #    location / {
    #        root   html;
    #        index  index.html index.htm;
    #    }
    #}

}
```
Live server address looks like： rtmp://host:1935/live
