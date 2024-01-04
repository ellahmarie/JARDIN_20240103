# Dataset Report Renderer
## A web service application that generates a report consisting of each row of JSON Array structured based on the given template.
This application will be needing two files for the dataset and template. It will throw an error if the dataset or template file is empty, or template file has no template-row tag found.

## Usage
To run the project using docker, please run the following commands. 
```
docker build -t <app_name> .
```
```
docker run <app-name>
```

### Endpoint: GET /api/report
#### Headers
- **content-type** - application/x-www-form-urlencoded
#### Request Parameters
  ![image](https://github.com/ellahmarie/JARDIN_20240103/assets/75961480/d27f32d1-82a1-4bd6-9d4b-3b3deb1337f8)
  - **dataset** (file): The dataset file must contain a JSON Array.
  ![image](https://github.com/ellahmarie/JARDIN_20240103/assets/75961480/6d4d1593-9396-4f76-918d-2f776b47e3fa)

  - **template** (file): The template file must have the template-row tag.
  ![image](https://github.com/ellahmarie/JARDIN_20240103/assets/75961480/d579a82f-47d4-4433-ab7f-f61061504ed3)


#### Response
  Returns report as a form of text
  ![image](https://github.com/ellahmarie/JARDIN_20240103/assets/75961480/4e53d994-2b7e-423e-9c70-0a56a20ba0d5)
