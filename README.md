# S3FileUploader

This application allows the user to upload files to amazon S3 storage without relying on the web interface, which might be subject to timeouts.

The user may choose to upload files sequentially, for larger files to ensure each file is uploaded without timeout, or in parallel, which is faster for multiple small files.

## Requirements

This project is built with VS2019.

The "Microsoft Visual Studio Installer Projects" extension is required to build the .Installer project, however for a local build, this is not required.

This project also uses the AWSSDK nuget package.

