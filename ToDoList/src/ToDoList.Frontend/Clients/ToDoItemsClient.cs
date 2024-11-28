namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.View;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemsView = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");
        toDoItemsView = response.Select(dto => new ToDoItemView
        {
            ToDoItemId = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        }).ToList();

        return toDoItemsView;
    }
}
