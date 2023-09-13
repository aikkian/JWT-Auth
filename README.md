# JWT-AUTH

## Description

The JWT Authentication .NET Sample is an example of an ASP.NET Web API designed to illustrate the implementation of role-based authentication using JWTs in a .Net Framework application.

The API has 1 controller:

* **AuthenticationController**: Contains the login and validate

### AuthenticationController

The `AuthenticationController` encompasses the login and validation APIs that we utilize to obtain and test JWT token authentication.

* POST `/authentication/login`

    * Returns the JWT token along with the user information from the database after the user enters their username and password.
    * Post Http Request Link: `https://<YOUR-DOMAIN:PORT>//authentication/login`
    * Request Body Example:

        ```json
        {
            "username": "suelynn",
            "password": "abc123"
        }
        ```

    * Response Body Example:

        ```json
	    {
	        "username": "suelynn",
	        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN1ZWx5bm4iLCJyb2xlIjpbIlNlbmlvciBTb2Z0d2FyZSBFbmdpbmVlciIsIlByb2R1Y3QgT3duZXIiXSwibmJmIjoxNjk0NTMxNDAxLCJleHAiOjE2OTQ1MzMyMDEsImlhdCI6MTY5NDUzMTQwMX0.     qqVN-K0AjVzefvvTIZvDxkVWhr_E4FnYfyJ5CwUmdgs",
    	    "role": [ "Senior Software Engineer", "Product Owner" ],
	        "status": Success,
	        "message": "User is valid"
	    }
        ```

* POST `/authentication/validate`

    * Validate the token using bearer token
    * Post Http Request Link: `https://<YOUR-DOMAIN:PORT>/authentication/validate`
    * Request Header Example:

        `Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN1ZWx5bm4iLCJyb2xlIjpbIlNlbmlvciBTb2Z0d2FyZSBFbmdpbmVlciIsIlByb2R1Y3QgT3duZXIiXSwibmJmIjoxNjk0NTMxNDAxLCJleHAiOjE2OTQ1MzMyMDEsImlhdCI6MTY5NDUzMTQwMX0.qqVN-K0AjVzefvvTIZvDxkVWhr_E4FnYfyJ5CwUmdgs`

    * Response Body Example:

        ```json
        {
	        "username": "suelynn",
	        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN1ZWx5bm4iLCJyb2xlIjpbIlNlbmlvciBTb2Z0d2FyZSBFbmdpbmVlciIsIlByb2R1Y3QgT3duZXIiXSwibmJmIjoxNjk0NTMxNDAxLCJleHAiOjE2OTQ1MzMyMDEsImlhdCI6MTY5NDUzMTQwMX0.qqVN-K0AjVzefvvTIZvDxkVWhr_E4FnYfyJ5CwUmdgs",
	        "role": [ "Senior Software Engineer", "Product Owner" ],
	        "status": Success,
	        "message": "Token is valid"
	    }   
        ```