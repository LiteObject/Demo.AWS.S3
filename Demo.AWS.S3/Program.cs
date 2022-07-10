using Amazon.S3;
using Amazon.S3.Model;

/* ****************************************************************************************************
 * BEFORE RUNNING THIS APP:
 * - Credentials must be specified in an AWS profile. If you use a profile other than the [default] profile, also set the AWS_PROFILE environment variable.
 * - An AWS Region must be specified either in the [default] profile or by setting the AWS_REGION environment variable. 
 * 
 * ORIGINAL SOURCE:
 * https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/quick-start-s3-1-winvs.html
 *****************************************************************************************************/

internal class Program
{

    private static async Task Main(string[] args)
    {
        Environment.SetEnvironmentVariable("AWS_PROFILE", "default");
        Environment.SetEnvironmentVariable("AWS_REGION", "us-east-2");

        // Create an S3 client object.
        AmazonS3Client s3Client = new();

        // List the buckets owned by the user. Call a class method that calls the API method.
        Console.WriteLine("\nGetting a list of your buckets...");
        var listResponse = await MyListBucketsAsync(s3Client);

        Console.WriteLine($"Number of buckets: {listResponse.Buckets.Count}");

        foreach (S3Bucket b in listResponse.Buckets)
        {
            Console.WriteLine(b.BucketName);
        }
    }

    // Async method to get a list of Amazon S3 buckets.
    static async Task<ListBucketsResponse> MyListBucketsAsync(IAmazonS3 s3Client)
    {
        return await s3Client.ListBucketsAsync();
    }

    static async Task<PutBucketResponse?> CreateS3Bucket(IAmazonS3 s3Client, string bucketName)
    {
        // If a bucket name was supplied, create the bucket. Call the API method directly

        PutBucketResponse? createResponse = null;

        try
        {
            Console.WriteLine($"\nCreating bucket {bucketName}...");
            createResponse = await s3Client.PutBucketAsync(bucketName);
            Console.WriteLine($"Result: {createResponse.HttpStatusCode.ToString()}");

        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Caught exception when creating a bucket: {e.Message}", e);
        }

        return createResponse;
    }
}