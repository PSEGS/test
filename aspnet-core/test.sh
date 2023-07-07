#!/bin/bash

# Name of the Docker container to force stop and remove
CONTAINER_NAME="testcontainer"

# Force stop the container using the specified name
CONTAINER_ID=$(docker ps -aqf "name=${CONTAINER_NAME}")
if [ -n "$CONTAINER_ID" ]; then
    echo "Force stopping container ${CONTAINER_NAME}..."
    docker stop force $CONTAINER_ID
else
    echo "Container ${CONTAINER_NAME} not found."
fi

# Remove the container using the specified name
CONTAINER_ID=$(docker ps -aqf "name=${CONTAINER_NAME}")
if [ -n "$CONTAINER_ID" ]; then
    echo "Removing container ${CONTAINER_NAME}..."
    docker rm $CONTAINER_ID
else
    echo "Container ${CONTAINER_NAME} not found."
fi

# Path to the directory containing the Dockerfile
DOCKERFILE_DIR="/var/lib/jenkins/workspace/test/aspnet-core"

# Name of the Docker image to remove
IMAGE_NAME="test"

# Remove images with the specified name
IMAGES=$(docker images --filter "reference=${IMAGE_NAME}" --format "{{.ID}}")
if [ -n "$IMAGES" ]; then
    echo "Removing images..."
    docker rmi $IMAGES
else
    echo "No images found with the name ${IMAGE_NAME}."
fi

# Name for the Docker image
IMAGE_NAME="test"

# Build the Docker image
echo "Building Docker image..."
docker build -t $IMAGE_NAME $DOCKERFILE_DIR

echo "Docker image created successfully."


# Name of the Docker image to create a container from
IMAGE_NAME="test"

# Name for the Docker container
CONTAINER_NAME="testcontainer"

# Create the Docker container
echo "Creating Docker container..."
docker run -dp 200:80 --name $CONTAINER_NAME $IMAGE_NAME

echo "Docker container created successfully."

exit 0



echo "Done."
