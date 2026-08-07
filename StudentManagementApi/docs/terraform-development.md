# Development infrastructure with Terraform

This deploys only `development`: private S3 + CloudFront for Angular, API Gateway + Lambda, private RDS SQL Server Express, VPC, Secrets Manager and CloudWatch. CloudFront is the only public origin, so the frontend calls `/api` on the same HTTPS host and the strict authentication cookies work without a custom domain.

## Bootstrap once

Install Terraform and authenticate the AWS CLI as an administrator. From the repository root:

```powershell
$AwsRegion = "us-east-1"
$Account = aws sts get-caller-identity --query Account --output text
$StateBucket = "student-management-tf-state-$Account-$AwsRegion"

terraform -chdir=StudentManagementApi/infrastructure/terraform/bootstrap init
terraform -chdir=StudentManagementApi/infrastructure/terraform/bootstrap apply `
  -var="aws_region=$AwsRegion" `
  -var="state_bucket_name=$StateBucket"
```

If the account already has the GitHub OIDC provider, import it before applying instead of creating another provider.

## GitHub

Create the `Development` GitHub Environment and set these variables:

| Variable | Value |
| --- | --- |
| `AWS_REGION` | `us-east-1` |
| `AWS_ROLE_ARN` | `github_deployer_role_arn` Terraform output |
| `TF_STATE_BUCKET` | `state_bucket_name` Terraform output |

Push a change to `development`. The workflow builds the Lambda and Angular app, applies Terraform, uploads the Angular output to its private S3 bucket and invalidates CloudFront.

## Database

RDS is intentionally private. Terraform creates the instance and application secret, but does not run EF migrations automatically. Apply the existing migration from a network location that can reach RDS before using the API.

RDS SQL Server and the VPC Secrets Manager endpoint are the primary recurring costs. Delete the development stack when no longer needed:

```powershell
terraform -chdir=StudentManagementApi/infrastructure/terraform/development destroy
```

Do not destroy the bootstrap state bucket unless you also want to lose Terraform state.
