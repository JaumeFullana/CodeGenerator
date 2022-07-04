# CodeGenerator

The CodeGenerator is an API/Web Service that has a simple function, generate a random code of 6 numbers, send it via email and then return it to the program that made the API call.

Before being able to perform this function, the user must connect to the web API through an operation with a POST request on the link:

```diff
- baseURL/token 
```

In addition, in the body of this request, 3 key-value pairs must be sent in the form of x-www-form-urlencoded, which will be:

![image](https://user-images.githubusercontent.com/74202163/177178033-f558bde7-da02-4c32-91ec-9ed982dac60d.png)

Where the username and password values will be a valid username and password. This request will return a token that will last 24 hours, therefore, a new token must be requested every 24 hours.
Once the above is done, we can use the operation of generating a code through a POST request. In order to use this operation, the previously generated token must be passed in the Authorization header of the request, identifying it as a Bearer token. Then, you have to pass to the operation an email, a username and, optionally, the name of the website or the program that is calling the API through the URL. Therefore, it can be called in these two ways:

```diff
- baseURL/api/CodeGenerator/{userMail}/{user}

or

- baseURL/api/CodeGenerator/{userMail}/{user}?platform={platform}
```

Once the call is made, the API will generate a new random code of 6 numbers and will send that code by mail to the email address that has been sent to the operation. If everything goes correctly, it will return the code generated in an HTTP response with the code 200 (OK), in the event that the token used is incorrect or has expired, it will return the message: "Authorization for this request has been denied.” in an HTTP response with code 401(Unauthorized) and in case of other errors it will return an exception with the information of the error that occurred in an HTTP response with code 500 (Internal Server Error).

There is a login in this repository where you can see how to connect to this API. You can use the login and see how the API works. 

There is also a swagger web page where you can see and test the differents operations of this API.
