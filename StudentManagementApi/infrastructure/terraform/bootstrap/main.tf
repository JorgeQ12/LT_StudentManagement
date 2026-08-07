terraform {
  required_version = ">= 1.10.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
}

data "aws_caller_identity" "current" {}

resource "aws_s3_bucket" "state" {
  bucket = var.state_bucket_name
}

resource "aws_s3_bucket_versioning" "state" {
  bucket = aws_s3_bucket.state.id

  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "state" {
  bucket = aws_s3_bucket.state.id

  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_public_access_block" "state" {
  bucket = aws_s3_bucket.state.id
  block_public_acls = true
  block_public_policy = true
  ignore_public_acls = true
  restrict_public_buckets = true
}

resource "aws_iam_openid_connect_provider" "github" {
  url = "https://token.actions.githubusercontent.com"
  client_id_list = ["sts.amazonaws.com"]
}

resource "aws_iam_role" "github_deployer" {
  name = "student-management-development-terraform"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect    = "Allow"
      Action    = "sts:AssumeRoleWithWebIdentity"
      Principal = { Federated = aws_iam_openid_connect_provider.github.arn }
      Condition = {
        StringEquals = { "token.actions.githubusercontent.com:aud" = "sts.amazonaws.com" }
        StringLike = {
          "token.actions.githubusercontent.com:sub" = [
            "repo:JorgeQ12/LT_StudentManagement:environment:Development",
            "repo:JorgeQ12/LT_StudentManagement:ref:refs/heads/development",
            "repo:JorgeQ12@77177063/LT_StudentManagement@1324539433:environment:Development",
            "repo:JorgeQ12@77177063/LT_StudentManagement@1324539433:ref:refs/heads/development"
          ]
        }
      }
    }]
  })
}

resource "aws_iam_role_policy" "github_deployer" {
  name = "manage-development-infrastructure"
  role = aws_iam_role.github_deployer.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{ Effect = "Allow", Action = ["apigateway:*", "cloudfront:*", "ec2:*", "lambda:*", "logs:*", "rds:*", "secretsmanager:*", "s3:*", "iam:GetRole", "iam:GetRolePolicy", "iam:CreateRole", "iam:DeleteRole", "iam:PassRole", "iam:PutRolePolicy", "iam:DeleteRolePolicy", "iam:AttachRolePolicy", "iam:DetachRolePolicy", "iam:TagRole", "iam:UntagRole", "iam:ListRolePolicies", "iam:ListAttachedRolePolicies"], Resource = "*" }]
  })
}

output "state_bucket_name" { value = aws_s3_bucket.state.bucket }
output "github_deployer_role_arn" { value = aws_iam_role.github_deployer.arn }
