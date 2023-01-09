import { TwitterApi } from 'twitter-api-v2';



const getTweets = async (twitterClient: TwitterApi) => {
    const data = await twitterClient.v2.search({query: "#dotnet", max_results: 10})
    console.log(data);
    await twitterClient.v1.tweet('Hello, this is a test.');
    // rest of the code here
  };
