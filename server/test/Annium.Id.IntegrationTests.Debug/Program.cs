using System.Threading.Tasks;
using Annium.Id.IntegrationTests.Controllers;

namespace Annium.Id.IntegrationTests.Debug
{
    public class Program
    {
        public static async Task Main()
        {
            await new MeControllerTest().RestoreMyAccess_ValidEmail_Ok();
        }
    }
}