namespace Yawat.Models
{
    [System.SerializableAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class ArrayOfTodoItemDto
    {
        private ArrayOfTodoItemDtoTodoItemDto[] todoItemDtoField;

        [System.Xml.Serialization.XmlElementAttribute("TodoItemDto")]
        public ArrayOfTodoItemDtoTodoItemDto[] TodoItemDto
        {
            get => this.todoItemDtoField;

            set => this.todoItemDtoField = value;
        }
    }

    [System.SerializableAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
#pragma warning disable SA1402 // File may only contain a single type
    public class ArrayOfTodoItemDtoTodoItemDto
#pragma warning restore SA1402 // File may only contain a single type
    {
        private byte idField;

        private string nameField;

        private bool isCompleteField;

        public byte Id
        {
            get => this.idField;
            set => this.idField = value;
        }

        public string Name
        {
            get => this.nameField;
            set => this.nameField = value;
        }

        public bool IsComplete
        {
            get => this.isCompleteField;
            set => this.isCompleteField = value;
        }
    }
}
