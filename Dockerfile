# Use an official Python runtime as a parent image
FROM python:3.9-slim

# Set the working directory in the container
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY backend/ /app

# Install any needed packages specified in requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Expose the port the Flask app runs on
EXPOSE 5000

# Define environment variables (default values can be overridden)
ENV MONGO_CONNECTION_STRING="mongodb://mongoadmin:secret@mongodb-container:27017/"
ENV MONGO_DATABASE_NAME="resume_challenge"

# Run app.py when the container launches
CMD ["python", "main.py"]
