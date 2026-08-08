# Development infrastructure with Terraform

This deploys only `development`: private S3 + CloudFront for Angular, API Gateway + Lambda, private RDS SQL Server Express, VPC, Secrets Manager and CloudWatch. CloudFront is the only public origin, so the frontend calls `/api` on the same HTTPS host and the strict authentication cookies work without a custom domain.

The default database is `db.t3.micro`, the eligible Free Tier size for RDS SQL Server Express. The Secrets Manager interface VPC endpoint is not a Free Tier resource; it has a small hourly charge, normally covered by the new-account credits but still consumes them.

## Bootstrap once

Install Terraform and authenticate the AWS CLI as an administrator. From the repository root:

```powershell
$AwsRegion = "us-east-1"
$Account = aws sts get-caller-identity --query Account --output text
$StateBucket = "student-management-tf-state-$Account-$AwsRegion"

terraform -chdir=infrastructure/terraform/bootstrap init
terraform -chdir=infrastructure/terraform/bootstrap apply `
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

RDS is intentionally private. In Development, the existing API Lambda applies pending EF Core migrations during startup and then creates the initial administrator if it does not exist. The operation is idempotent and controlled by Terraform through `apply_database_migrations`.

The administrator email defaults to `admin@studentmanagement.local`. Its generated password is stored only in the application secret. Retrieve it after deployment without printing it in GitHub Actions:

```powershell
$SecretName = terraform -chdir=infrastructure/terraform/development output -raw application_secret_name
$ApplicationSecret = aws secretsmanager get-secret-value `
  --region us-east-1 `
  --secret-id $SecretName `
  --query SecretString `
  --output text | ConvertFrom-Json

$ApplicationSecret.BootstrapAdministratorPassword
```

For Production, migrations should run as an explicit deployment operation rather than during normal API startup.

RDS SQL Server and the VPC Secrets Manager endpoint are the primary recurring costs. Delete the development stack when no longer needed:

```powershell
terraform -chdir=infrastructure/terraform/development destroy
```

Do not destroy the bootstrap state bucket unless you also want to lose Terraform state.
