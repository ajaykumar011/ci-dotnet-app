using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Threading.Tasks;
     
namespace App
{
  class Program
  {
   async static Task Main(string[] args)
   {
         
      var bucketRegion = RegionEndpoint.APSouth1;
      // Note: You could add more valid AWS Regions above as needed.
     
      using (var s3Client = new AmazonS3Client(bucketRegion)) {
      var bucketName = args[0];       
             
        // List current buckets.
       Console.WriteLine("\nMy buckets now are:");
       var response = await s3Client.ListBucketsAsync();
        
       foreach (var bucket in response.Buckets)
       {
       Console.WriteLine(bucket.BucketName);
       }
      }
    }
  }
}