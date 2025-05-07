namespace todo_backend.Dto
{
    public class CreateTodoRequest
    {
        public required String Title { get; set; }
        public required String Description{ get; set; }
    }public class UpdateTodoRequest
    {
        public required Boolean Completion { get; set; }
    }
}
