output "frontend_url" { value = "https://${aws_cloudfront_distribution.frontend.domain_name}" }
output "api_url" { value = "https://${aws_cloudfront_distribution.frontend.domain_name}/api" }
output "swagger_url" { value = "https://${aws_cloudfront_distribution.frontend.domain_name}/swagger" }
output "frontend_bucket_name" { value = aws_s3_bucket.frontend.bucket }
output "database_endpoint" { value = aws_db_instance.sql_server.address }
output "cloudfront_distribution_id" { value = aws_cloudfront_distribution.frontend.id }
