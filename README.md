# README
***
This is an example of how to connect to AWS IoT Core using C# and .NET Core.

## 1. Manualy creating a Thing on AWS IoT
1. Once you are loged in the [AWS Console](https://us-east-2.console.aws.amazon.com/console/home?nc2=h_ct&src=header-signin&region=us-east-2) click on the **IoT Core** services 
Now on the left pane click on Manage -> Things and then, in the right pane click on the Create button in order to create a new thing.
![](/Images/IoTThings.png)
![](/Images/IoTCreateThing.png)

Click on create a single thing. Then choose a name and leave everything else as default and click next. Click on **Create certificate**. 

Now download all the three files and rename them as follows:
-Rename the **Device Certificate** (file with .pem.crt extension) as _certificate.cert.pem_
-Rename the **Public Key** (file with .pem.key) as _certificate.public.key_.
-Rename the **Private Key** (file with .pem.key) as _certificate.private.key_.

At the end you should have the files as shown in the next image.
![](/Images/RenamedFiles.png)

Then click on **Activate** and then click on **Done**.

Now download the [Amazon Root CA 1](https://www.amazontrust.com/repository/AmazonRootCA1.pem) certificate and save it as _AmazonRootCA1.crt_.

## 2. Creating a Thing Policy.
Once you've created the Thing we need to create a policy. Follow the steps to create and attach a new policy to it.
1. On the left pane click on **Secure** -> **Policies**. Then on the right pane click on **create**
2. Give a name to your policy. In this case it is named _MyThingPolicy_.
In the section *Add Statemants*.
![](/Images/PolicyName.png)

On the *Add Statemants* section in the accion field type _iot:Connect_ and on the _Resource ARN_ type _arn:aws:iot:us-east-2:255278214407:client/myThingID_. Then on the _Effect_ checkbox check **Allow**.
 
Then click on the button **Add new statement** and type _iot:Publish_ on the action field, _arn:aws:iot:us-east-2:255278214407:topic/MyThingTopic_ on the Resource ARN field and check the **Allow** box.
 
Click once again on the button **Add new statement** and type _iot:Receive_ on the action field, _arn:aws:iot:us-east-2:255278214407:topic/MyThingTopic_ on the Resource ARN field and check the **Allow** box.
 
Click one last time on the button **Add new statement** and type _iot:Subscribe_ on the action field, _arn:aws:iot:us-east-2:255278214407:topicfilter/MyThingTopic_ on the Resource ARN field and check the **Allow** box.

At the end of the day you should have something like as shown in the image below
![](/Images/policy.png)

Now click on **create**.
3. To attach our new policy to the Thing we've created go to _Manage_ -> _Things_. Click on _NewThing_ then go to _secure_	and click on the certificate

![](/Images/ThingPolicy.png)

Go to _Policies_and on the **Acctions** drop down button click on *attach policy*.
![](/Images/ThingAttachPolicy.png)

and choose the policy we've created. Now the policy is correctly attach to the thing certificate.

## 3. Converting the Device Certificate from .pem to .pfx
In order to establish a MQTT Connection with AWS IoT platform we need to convert the device certificate we downloaded in section 2 to .pfx. To do so open your bash at the path where your certificates are located
and then use the following command

```
openssl pkcs12 -export -in **iotdevicecertificateinpemformat** -inkey **iotdevivceprivatekey** -out **devicecertificateinpfxformat** -certfile **rootcertificatefile**
```

if you named the files as suggested before then it will look like below

```
openssl pkcs12 -export -in certificate.cert.pem -inkey certificate.private.key -out certificate.cert.pfx -certfile AmazonRootCA1.crt
```

Then when it ask you for a password just press _Enter_ twice and then you will have the .pfx certificate in your folder.

## 4. Using C# to connect to our AWS IoT Thing
Clone this repository and copy all your certificates to the _Certificates_ folder. 
Now open the solution and you will have something as below
![](/Images/code.png)

To set the enpoint value we need to get a prefix and the region, to do so go to the *AWS IoT Core Console* and go to _Manage_ -> _Things_ and choose the thing we've created.
then go to _Interact_ and you will find something as below
![](/Images/prefixAndRegion.png)

The text right before the first dash is the prefix you should replace for the _prefix_ field in the iotEndpoint string  and replace the region for the one that appears in your URL,
in this case _us-east-2_. You should get something as

``` C#
string iotEndpoint = "a55ysl12e68g1t.iot.us-east-2.amazonaws.com";
```

To get your *Unique Client ID* type the id you wrote on your policy for the _Connect statement_. In this case it was _myThingID_. You should have something like this
```C#
string clientID = "myThingID"; 
```

Now everything is set up. Just build the project and run it and you should be able to establish the connection with AWS IoT Core Succesfuly.
