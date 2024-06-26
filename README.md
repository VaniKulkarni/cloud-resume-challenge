# cloud-resume-challenge

This project contains source code and supporting files for a serverless application that you can deploy with the SAM CLI. It includes the following files and folders.

- frontend - Code for the application's front end
- backend - Code for the application's Lambda function.
- test - Cyopress end to end tests for the deployed application code.
- template.yaml - A template that defines the application's AWS resources.
- github\workflows - On push\manual trigger Deploy end-to-end solution on AWS and test

The application uses several AWS resources, including Lambda functions and an API Gateway API. These resources are defined in the `template.yaml` file in this project. You can update the template to add AWS resources through the same deployment process that updates your application code.

## Setup AWS SAM and deploy first time

The Serverless Application Model Command Line Interface (SAM CLI) is an extension of the AWS CLI that adds functionality for building and testing Lambda applications. It uses Docker to run your functions in an Amazon Linux environment that matches Lambda. It can also emulate your application's build environment and API.

To use the SAM CLI, you need the following tools.

- SAM CLI - [Install the SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install.html)

To build and deploy your application for the first time, run the following in your shell:

```bash
sam build
sam deploy --guided
```

The first command will build the source of your application. The second command will package and deploy your application to AWS, with a series of prompts:

- **Stack Name**: The name of the stack to deploy to CloudFormation. This should be unique to your account and region, and a good starting point would be something matching your project name.
- **AWS Region**: The AWS region you want to deploy your app to.
- **Confirm changes before deploy**: If set to yes, any change sets will be shown to you before execution for manual review. If set to no, the AWS SAM CLI will automatically deploy application changes.
- **Allow SAM CLI IAM role creation**: Many AWS SAM templates, including this example, create AWS IAM roles required for the AWS Lambda function(s) included to access AWS services. By default, these are scoped down to minimum required permissions. To deploy an AWS CloudFormation stack which creates or modifies IAM roles, the `CAPABILITY_IAM` value for `capabilities` must be provided. If permission isn't provided through this prompt, to deploy this example you must explicitly pass `--capabilities CAPABILITY_IAM` to the `sam deploy` command.
- **Save arguments to samconfig.toml**: If set to yes, your choices will be saved to a configuration file inside the project, so that in the future you can just re-run `sam deploy` without parameters to deploy changes to your application.

You can find your API Gateway Endpoint URL in the output values displayed after deployment.

## Use the SAM CLI to build and deploy

Build your application with the `sam build` command.

```bash
cloud-resume-challenge$ sam build && sam deploy
cloud-resume-challenge$ aws s3 sync ./frontend s3://vani.kulkarnisworklife.uk
```

The SAM CLI installs dependencies defined in `backend/PutCount.csproj`, creates a deployment package, and saves it in the `.aws-sam/build` folder.

## Add a resource to your application

Edit template.yaml file

The application template uses AWS Serverless Application Model (AWS SAM) to define application resources. AWS SAM is an extension of AWS CloudFormation with a simpler syntax for configuring common serverless application resources such as functions, triggers, and APIs. For resources not included in [the SAM specification](https://github.com/awslabs/serverless-application-model/blob/master/versions/2016-10-31.md), you can use standard [AWS CloudFormation](https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/aws-template-resource-type-ref.html) resource types.

## How is Cloudflare setup done

Cloudflare is used for domain registration, content delivery network (CDN), and SSL certificate provisioning.

While it offers secure HTTPS connections between users and its servers, the connection between Cloudflare and S3 (e.g., for hosting a website) isn't encrypted.
This setup, commonly used with CNAME records in Cloudflare, provides security for user traffic but not for data transmission between Cloudflare and S3.

[Reference](https://advancedweb.hu/how-to-add-https-for-an-s3-bucket-website-with-cloudflare/)

## To run Cypress E2E tests locally

Tests are defined in the `test` folder in this project.

```bash
cloud-resume-challenge\test$ npm run cypress:open
```

## Cleanup

To delete the sample application that you created, use the AWS CLI. Assuming you used your project name for the stack name, you can run the following:

```bash
cloud-resume-challenge$ sam delete --stack-name cloud-resume-challenge
cloud-resume-challenge$ aws s3 rm s3://vani.kulkarnisworklife.uk --recursive
```
