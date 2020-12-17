namespace Lawful.Core.Modelo.TareaEstados
{
    public class EnCurso : Interfaces.IEstadoTarea
    {
        public int DBValue { get => 2; }
        public Tarea Tarea { get; set; }

        public void Mover()
        {
            Tarea.ChangeState(new Finalizada());
        }

        public void MoverAtras()
        {
            Tarea.ChangeState(new PorHacer());
        }

        public void SetTarea(Tarea tarea)
        {
            Tarea = tarea;
        }
    }
}
