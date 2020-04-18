# CoreFaceDetector

Face finding web service on net core 3.1 and https://github.com/shimat/opencvsharp

Just run and give it a photo from internet: `http://localhost:59642/face/detect?uri=http://example.com/picture.jpg`

Or from the file system `uri=file://C:/downloads/picture.jpg`

Response is 404 if there no faces or biggest face `{"rectangle":"829,254,570,570"}`
