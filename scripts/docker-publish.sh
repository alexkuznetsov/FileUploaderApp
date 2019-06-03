DOCKER_ENV=''
DOCKER_TAG=''

case "$TRAVIS_BRANCH" in
  "master")
    DOCKER_ENV=production
    DOCKER_TAG=latest
    ;;
  "dev")
    DOCKER_ENV=development
    DOCKER_TAG=dev
    ;;    
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -t fileuploaderapp:$DOCKER_TAG .
docker tag fileuploaderapp:$DOCKER_TAG $DOCKER_USERNAME/fileuploaderapp:$DOCKER_TAG
docker push $DOCKER_USERNAME/fileuploaderapp:$DOCKER_TAG
