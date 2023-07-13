using API.DTOs.University;
using API.Models;
using API.Utilities.Handler;
using Client.Contracts;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Client.Repository;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(string request="University/") : base(request)
    {
    }

    public async Task<ResponseHandler<GetUniversityDto>> Put(Guid guid, GetUniversityDto universityDto)
    {
        ResponseHandler<GetUniversityDto> entityVM = null;

        var requestPayload = new
        {
            guid = guid.ToString(),
            code = universityDto.Code,
            name = universityDto.Name
        };

        StringContent content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

        using (var response = await _httpClient.PutAsync(_request, content))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(apiResponse); // Add this line for logging
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<GetUniversityDto>>(apiResponse);
        }

        return entityVM;
    }


}
