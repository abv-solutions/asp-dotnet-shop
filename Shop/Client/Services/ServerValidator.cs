using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Shop.Client.Services
{
    public partial class ServerValidator : ComponentBase
    {
        [CascadingParameter]
        EditContext editContext { get; set; }
        ValidationMessageStore messageStore { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (editContext == null)
                throw new InvalidOperationException($"{nameof(ServerValidator)} requires a cascading parameter of type " +
                    $"{nameof(editContext)}. For example, you can use {nameof(ServerValidator)} inside an EditForm.");

            messageStore = new ValidationMessageStore(editContext);
        }

        public async void Validate(HttpResponseMessage res, object model)
        {
            if (res.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                var body = await res.Content.ReadAsStringAsync();
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body);

                if (problemDetails.Errors != null)
                {
                    messageStore.Clear();
                    foreach (var err in problemDetails.Errors)
                        messageStore.Add(new FieldIdentifier(model, err.Key), err.Value);
                }
            }

            editContext.NotifyValidationStateChanged();
        }

        public void Reset()
        {
            messageStore.Clear();
            editContext.NotifyValidationStateChanged();
        }
    }
}
