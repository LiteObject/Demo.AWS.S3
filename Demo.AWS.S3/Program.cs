using Amazon.S3;
using Amazon.S3.Model;

namespace Demo.AWS.S3
{
    /* ****************************************************************************************************
     * BEFORE RUNNING THIS APP:
     * - Credentials must be specified in an AWS profile. If you use a profile other than the [default] profile, also set the AWS_PROFILE environment variable.
     * - An AWS Region must be specified either in the [default] profile or by setting the AWS_REGION environment variable. 
     * 
     * ORIGINAL SOURCE:
     * https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/quick-start-s3-1-winvs.html
     * https://docs.aws.amazon.com/sdkfornet1/latest/apidocs/html/T_Amazon_S3_AmazonS3.htm
     *****************************************************************************************************/

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Environment.SetEnvironmentVariable("AWS_PROFILE", "default");
            Environment.SetEnvironmentVariable("AWS_REGION", "us-east-2");

            // Create an S3 client object.
            using AmazonS3Client s3Client = new();

            /*
             * var response = await CreateS3BucketAsync(s3Client, $"delete-this-bucket-{DateTime.Now.Ticks}");
             * var response = await DeleteS3BucketAsync(s3Client, "delete-this-bucket-637930518805186075");
                if (response != null)
                {
                    Console.WriteLine(response.ResponseMetadata.ToString());
                }
            */

            // List the buckets owned by the user. Call a class method that calls the API method.
            Console.WriteLine("\nGetting a list of your buckets...");
            var listResponse = await MyListBucketsAsync(s3Client);

            Console.WriteLine($"Number of buckets: {listResponse.Buckets.Count}");

            foreach (S3Bucket b in listResponse.Buckets)
            {
                Console.WriteLine(b.BucketName + " [CreatedOn: " + b.CreationDate + "]");
                await PrintBucketObjectsAsync(s3Client, b.BucketName);
            }
        }

        static async Task<ListBucketsResponse> MyListBucketsAsync(IAmazonS3 s3Client)
        {
            return await s3Client.ListBucketsAsync();
        }

        static async Task PrintBucketObjectsAsync(IAmazonS3 s3Client, string bucketName)
        {
            ListObjectsRequest listRequest = new()
            {
                BucketName = bucketName,
            };

            ListObjectsResponse listResponse;

            do
            {
                listResponse = await s3Client.ListObjectsAsync(listRequest);

                foreach (S3Object obj in listResponse.S3Objects)
                {
                    Console.WriteLine("------------------");
                    Console.WriteLine(" Object: " + obj.Key);
                    Console.WriteLine(" Size: " + obj.Size);
                    Console.WriteLine(" LastModified: " + obj.LastModified);
                    Console.WriteLine(" Storage class: " + obj.StorageClass);
                }

                // Set the marker property
                listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);
        }

        static async Task<PutBucketResponse?> CreateS3BucketAsync(IAmazonS3 s3Client, string bucketName)
        {
            PutBucketResponse? createResponse = null;

            try
            {
                Console.WriteLine($"\nCreating bucket {bucketName}...");
                createResponse = await s3Client.PutBucketAsync(bucketName);
                Console.WriteLine($"Result: {createResponse.HttpStatusCode}");

            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Caught exception when creating a bucket: {e.Message}", e);
            }

            return createResponse;
        }

        static async Task<DeleteBucketResponse?> DeleteS3BucketAsync(IAmazonS3 s3Client, string bucketName)
        {
            DeleteBucketResponse? deleteResponse = null;

            try
            {
                Console.WriteLine($"\nDeleting bucket {bucketName}...");
                deleteResponse = await s3Client.DeleteBucketAsync(bucketName);
                Console.WriteLine($"Result: {deleteResponse.HttpStatusCode}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Caught exception when deleting a bucket: {e.Message}", e);
            }

            return deleteResponse;
        }
    }
}