using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Protocolo;

namespace BatalhaNaval
{
    /// <summary>
    /// Gambiarra de drag and drop :p
    /// </summary>
    
    class NavioDirecionado
    {
        public TipoDeNavio Navio { get; private set; }

        public int Direcao { get; private set; }

        public NavioDirecionado(TipoDeNavio navio, int direcao)
        {
            Navio = navio;
            direcao = direcao % 3;
        }
    }

    class DragDropNavios
    {
        int direcao = 0;

        public int Direcao {
            get { return direcao; }
            set
            {
                direcao = value % 3;
            }
        }

        public TipoDeNavio Navio { get; set; }

        public bool IsDragging { get; set; }

        public DragDropNavios()
        {

        }

        public void DoDragDrop(TipoDeNavio navio)
        {
            Direcao = 0;
            IsDragging = true;
            Navio = navio;

            OnDragStart();
        }

        public NavioDirecionado Accept()
        {
            IsDragging = false;
            OnDragDrop();
            return new NavioDirecionado(Navio, Direcao);
        }

        public void ClearEvents()
        {
            foreach (DragDropCallback d in OnDragDrop.GetInvocationList())
                OnDragDrop -= d;

            foreach (DragDropCallback d in OnDragCancel.GetInvocationList())
                OnDragCancel -= d;

            foreach (DragDropCallback d in OnDragStart.GetInvocationList())
                OnDragStart -= d;
        }

        public delegate void DragDropCallback();

        public event DragDropCallback OnDragDrop;

        public event DragDropCallback OnDragCancel;

        public event DragDropCallback OnDragStart;
    }
}
