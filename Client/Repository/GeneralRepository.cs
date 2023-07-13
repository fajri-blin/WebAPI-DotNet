using API.Utilities.Handler;
using Client.Contracts;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Client.Repository;

public class GeneralRepository<TEntity> : IGeneralRepository<TEntity>
    where TEntity : class
{
    protected readonly string _request;
    protected readonly HttpClient _httpClient;

    public GeneralRepository(string request)
    {
        this._request = request;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7114/api/")
        };
    }

    public async Task<ResponseHandler<IEnumerable<TEntity>>> Get()
    {
        ResponseHandler<IEnumerable<TEntity>> entityVM = null;
        using(var response = await _httpClient.GetAsync(_request))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<TEntity>>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandler<TEntity>> Get(Guid guid)
    {
        ResponseHandler<TEntity> entity = null;


        using (var response = await _httpClient.GetAsync(_request + guid))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entity = JsonConvert.DeserializeObject<ResponseHandler<TEntity>>(apiResponse);
        }
        return entity;
    }

    public async Task<ResponseHandler<TEntity>> Post(TEntity entity)
    {
        ResponseHandler<TEntity> entityVm = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request, content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVm = JsonConvert.DeserializeObject<ResponseHandler<TEntity>>(apiResponse);
        }
        return entityVm;
    }

    public async Task<ResponseHandler<TEntity>> Delete(Guid guid)
    {
        ResponseHandler<TEntity> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(guid), Encoding.UTF8, "application/json");
        using (var response = _httpClient.DeleteAsync(_request + guid).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<TEntity>>(apiResponse);
        }
        return entityVM;
    }
}
