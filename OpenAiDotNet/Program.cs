

//Instalar o pacote Azure.AI.OpenAI beta 14

using Azure.AI.OpenAI;
using Azure;



class Program
{
    static void Main(string[] args)
    {
        //GPT35Async().Wait();
        WallEAsync().Wait();
    }

    static async Task GPT35Async()
    {
        // Configuração de credenciais
        OpenAIClient client = new OpenAIClient(
            new Uri("https://openai-vemcodar.openai.azure.com/"),
            new AzureKeyCredential("pegarNoAzure"));


        string fraseAtual = "Conte de 1 ate 5 e cite um nome feminino depois dos numeros pares!";
        string ultimaFraseUsuario = fraseAtual;
        string ultimaResposta;

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-35-turbo", // Nome que voce deu para a implementação do modelo escolhido
            Messages =
                {
                    // Pedido ao chatGpt
                    new ChatRequestSystemMessage(fraseAtual)
                }
        };

        Response<ChatCompletions> response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
        ChatResponseMessage responseMessage = response.Value.Choices[0].Message;

        ultimaResposta = $"[{responseMessage.Role.ToString().ToUpperInvariant()}]: {responseMessage.Content}";

        Console.WriteLine(ultimaResposta);

        Console.WriteLine();
        Console.WriteLine("-------------------------");
        Console.WriteLine("-------------------------");
        Console.WriteLine("-------------------------");
        Console.WriteLine();

        fraseAtual = "Substitua o numero 2 pela palavra Thamirys e mantenha o restante do texto igual";

        chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-35-turbo", // Nome que voce deu para a implementação do modelo escolhido
            Messages =
                {
                    // Ativando a memória do GPT sobre o ultimo pedido
                    new ChatRequestUserMessage(ultimaFraseUsuario),

                    // Ativando a memória do GPT sobre a ultima resposta
                    new ChatRequestAssistantMessage(ultimaResposta),                    

                    // pedido atual
                    new ChatRequestSystemMessage(fraseAtual),
                },
            ChoiceCount = 3,
        };

        response = await client.GetChatCompletionsAsync(chatCompletionsOptions);


        foreach (var chatCompletions in response.Value.Choices)
        {
            Console.WriteLine($"[{responseMessage.Role.ToString().ToUpperInvariant()}]: {chatCompletions.Message.Content}");

            Console.WriteLine("-------------------------");
            Console.WriteLine();
        }

        
        





    }

    static async Task WallEAsync()
    {
        // Configuração de credenciais
        OpenAIClient client = new OpenAIClient(
            new Uri("https://openai-vemcodar.openai.azure.com/"),
            new AzureKeyCredential("pegarNoAzure"));



        Response<ImageGenerations> response = await client.GetImageGenerationsAsync
        (
            new ImageGenerationOptions()
            {
                DeploymentName = "Dall-e-3",
                Prompt = "uma programadora da linguagem C#, irritada, de nacionalidade brasileira, de cabelos ruivos, gorda e de oculos com armação vinho",
                Size = ImageSize.Size1024x1024,
                Quality = ImageGenerationQuality.Standard
            }
        );

        ImageGenerationData generatedImage = response.Value.Data[0];
        if (!string.IsNullOrEmpty(generatedImage.RevisedPrompt))
        {
            Console.WriteLine($"Input prompt automatically revised to: {generatedImage.RevisedPrompt}");
        }

        Console.WriteLine();
        Console.WriteLine("Link da imagem:");
        Console.WriteLine($"{generatedImage.Url.AbsoluteUri}");

    }
}