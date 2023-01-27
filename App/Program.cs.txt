using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using System;
using System.Threading.Tasks;
//https://github.com/aws/aws-sdk-net/issues/2477
namespace App
{
    // docker run --rm -it -v "$(Get-Location):/src" -v "$env:USERPROFILE/.aws:/root/.aws:ro" -w /src mcr.microsoft.com/dotnet/core/sdk:3.1 dotnet run
    // Found 327 buckets.
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AWSCredentials credentials;

            if (Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") != null)
            {
                Console.WriteLine("Getting credentials from environment variables.");
                credentials = new EnvironmentVariablesAWSCredentials();
            }
            else
            {
                Console.WriteLine("Getting credentials from credentials files.");

                var profileName = args.Length == 0 ? "default" : args[0];

                var sharedFile = new SharedCredentialsFile();

                if (!sharedFile.TryGetProfile(profileName, out var profile))
                {
                    throw new ArgumentException($"AWS profile '{profileName}' was not found.");
                }

                if (!AWSCredentialsFactory.TryGetAWSCredentials(profile, sharedFile, out credentials))
                {
                    throw new NotSupportedException($"Failed to get AWS credentials from profile named '{profileName}'.");
                }
            }

            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.APSouth1);

            var response = await s3Client.ListBucketsAsync();
            Console.WriteLine($"Found {response.Buckets.Count} buckets.");
            await Task.Delay(TimeSpan.FromMilliseconds(1_000));
            Console.WriteLine($"Execution Completed");
        }
    }
}