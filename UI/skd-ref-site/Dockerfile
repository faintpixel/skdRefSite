FROM nginx

COPY ui-nginx.conf /etc/nginx/nginx.conf

WORKDIR /usr/share/nginx/html
COPY UI/skd-ref-site/dist/ .

# Questions:
#  - Where should this file live?
#    - I was thinking I'd put it in a docker folder, but then it can't reference the UI/skd-ref-site/dist folder so it errors
#    - If it has to be in this folder, where do I put the others? I guess I could put it in UI, API, and Database, but it seems weird to have the docker files spread out and mixed in with everything else
#  - Should I be calling ng build before line 6? 