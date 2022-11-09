using UnityEngine;
using Thirdweb;
using TMPro;

public class SDKTest : MonoBehaviour
{
    private ThirdwebSDK sdk;
    private int count;
    public TMP_Text loginButton;
    public TMP_Text resultText;

    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
    }

    void Update() {
    }

    public async void OnLoginCLick()
    {
        Debug.Log("Login clicked");
        
        loginButton.text = "Connecting...";
        string address = await sdk.Connect();
        loginButton.text = "Connected as: " + address.Substring(0, 6) + "...";
    }

    public async void GetERC721()
    {
        var contract = sdk.GetContract("0x2e01763fA0e15e07294D74B63cE4b526B321E389"); // NFT Drop
        Debug.Log("Button clicked");
        count++;
        resultText.text = "Fetching Token: " + count;
        NFT result = await contract.ERC721.Get(count.ToString());
        Debug.Log("name: " + result.metadata.name);
        Debug.Log("owner: " + result.owner);
        resultText.text = result.metadata.name;
        // int supply = await contract.ERC721.TotalClaimedSupply();
        // fetchButton.text = supply.ToString();
        // string uri = await contract.Read<string>("tokenURI", count);
        // fetchButton.text = uri;
    }

    public async void GetERC1155()
    {
        var contract = sdk.GetContract("0x86B7df0dc0A790789D8fDE4C604EF8187FF8AD2A"); // Edition Drop
        Debug.Log("Button clicked");
        count++;
        resultText.text = "Fetching Token: " + count;
        NFT result = await contract.ERC1155.Get(count.ToString());
        resultText.text = result.metadata.name + " (x" + result.quantityOwned + ")";
    }

    public async void GetERC20()
    {
        var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        Debug.Log("Button clicked");
        resultText.text = "Fetching Token info";
        Currency result = await contract.ERC20.Get();
        CurrencyValue currencyValue = await contract.ERC20.TotalSupply();
        Debug.Log("name: " + result.name);
        resultText.text = result.name + " (" + currencyValue.displayValue + ")";
    }

    public async void MintERC721()
    {
        var contract = sdk.GetContract("0x2e01763fA0e15e07294D74B63cE4b526B321E389"); // NFT Drop
        Debug.Log("Claim button clicked");
        //var result = await contract.ERC721.Transfer("0x2247d5d238d0f9d37184d8332aE0289d1aD9991b", count.ToString());
        resultText.text = "claiming...";
        var result = await contract.ERC721.Claim(1);
        Debug.Log("result id: " + result[0].id);
        Debug.Log("result receipt: " + result[0].receipt.transactionHash);
        resultText.text = "claimed tokenId: " + result[0].id;
        
        // sig mint
        // var contract = sdk.GetContract("0x8bFD00BD1D3A2778BDA12AFddE5E65Cca95082DF"); // NFT Collection
        // var meta = new NFTMetadata();
        // meta.name = "Unity NFT";
        // meta.description = "Minted From Unity (signature)";
        // meta.image = "ipfs://QmbpciV7R5SSPb6aT9kEBAxoYoXBUsStJkMpxzymV4ZcVc";
        // var payload = new ERC721MintPayload("0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", meta);
        // var p = await nftCollection.ERC721.signature.Generate(payload);
        // var result = await nftCollection.ERC721.signature.Mint(p);
        // resultText.text = "sigminted tokenId: " + result.id;
    }

    public async void MintERC1155()
    {
        Debug.Log("Claim button clicked");
        resultText.text = "claiming...";

        // claim
        // var contract = sdk.GetContract("0x86B7df0dc0A790789D8fDE4C604EF8187FF8AD2A"); // Edition Drop
        // var result = await contract.ERC1155.Claim("0", 1);
        // Debug.Log("result receipt: " + result[0].receipt.transactionHash);
        // resultText.text = "claim successful";


        // sig mint
        var contract = sdk.GetContract("0xdb9AAb1cB8336CCd50aF8aFd7d75769CD19E5FEc"); // Edition
        //  var meta = new NFTMetadata();
        // meta.name = "Kitten Unity NFT";
        // meta.description = "Cat Minted From Unity (with signature)";
        // meta.image = "ipfs://QmbpciV7R5SSPb6aT9kEBAxoYoXBUsStJkMpxzymV4ZcVc";
        // var payload = new ERC1155MintPayload(meta);
        var payload = new ERC1155MintAdditionalPayload("0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", "1");
        payload.quantity = 3;
        var p = await contract.ERC1155.signature.GenerateFromTokenId(payload);
        var result = await contract.ERC1155.signature.Mint(p);
        resultText.text = "sigminted tokenId: " + result.id;
    }

     public async void MintERC20()
    {
        Debug.Log("Claim button clicked");
        resultText.text = "claiming...";

        // Mint
        var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Edition Drop
        var result = await contract.ERC20.Mint("1.2");
        Debug.Log("result receipt: " + result.receipt.transactionHash);
        resultText.text = "mint successful";


        // sig mint
        // var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        // var payload = new ERC20MintPayload("0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", "3.2");
        // var p = await contract.ERC20.signature.Generate(payload);
        // await contract.ERC20.signature.Mint(p);
        // resultText.text = "sigminted currency successfully";
    }
}
