using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Love.Net.Core;
using Microsoft.AspNetCore.Mvc;
using RigoFunc.ApiCore;

namespace Host.Controllers {
    [Route("api/[controller]")]
    public class ValuesController : ApiController {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public DateTime Get(int id) {
            return DateTime.Now;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value) {
        }

        [HttpPost("[action]")]
        public User NULL([FromBody]string value) {
            return null;
        }

        [HttpPost("[action]")]
        public async Task TaskVoid([FromBody]string value) {
            await Task.Delay(0);
        }

        [HttpPost("[action]")]
        public async Task Error([FromBody]string value) {
            InvokeError.Caused("xUnit Test").Throw();
            await Task.Delay(0);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
