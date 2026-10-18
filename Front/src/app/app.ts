import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from './services/api';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App implements OnInit {
  materias: any[] = [];
  estudiantes: any[] = [];

  // Datos del formulario
  nombreEstudiante: string = '';
  correoEstudiante: string = '';
  materiasSeleccionadas: number[] = [];

  mensajeError: string = '';
  mensajeExito: string = '';

  constructor(
    private apiService: ApiService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.cargarDatos();
  }

 cargarDatos() {
    this.apiService.getMaterias().subscribe({
      next: (data) => {
        this.materias = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Error al cargar materias', err)
    });

    this.apiService.getEstudiantes().subscribe({
      next: (data) => {
        this.estudiantes = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Error al cargar estudiantes', err)
    });
  }

  
  onMateriaChange(materiaId: number, event: any) {
    const isChecked = event.target.checked;
    this.mensajeError = ''; 

    if (isChecked) {
      
      if (this.materiasSeleccionadas.length >= 3) {
        this.mensajeError = '⚠️ Solo puedes seleccionar un máximo de 3 materias (9 créditos).';
        event.target.checked = false; 
        this.cdr.detectChanges();
        return;
      }

      
      const materiaSeleccionada = this.materias.find((m: any) => m.id === materiaId);
      if (!materiaSeleccionada) return;

      
      const tieneMismoProfesor = this.materiasSeleccionadas.some((idSeleccionada: number) => {
        const matExistente = this.materias.find((m: any) => m.id === idSeleccionada);
        return matExistente && matExistente.profesorId === materiaSeleccionada.profesorId;
      });

      if (tieneMismoProfesor) {
        this.mensajeError = `🚫 No puedes inscribir dos materias con el mismo profesor (${materiaSeleccionada.profesorNombre || 'Profesor'}). Cada materia debe ser con un profesor diferente.`;
        event.target.checked = false; 
        this.cdr.detectChanges();
        return;
      }

      
      this.materiasSeleccionadas.push(materiaId);
    } else {
      // Si se desmarca, remover de la lista
      this.materiasSeleccionadas = this.materiasSeleccionadas.filter((id: number) => id !== materiaId);
    }

    
    this.cdr.detectChanges();
  }

  
registrar() {
    this.mensajeError = '';
    this.mensajeExito = '';

    // Validar campos obligatorios
    if (!this.nombreEstudiante || this.nombreEstudiante.trim() === '' || 
        !this.correoEstudiante || this.correoEstudiante.trim() === '') {
      this.mensajeError = '⚠️ Por favor, completa el nombre y el correo electrónico.';
      this.cdr.detectChanges();
      return;
    }

    if (this.materiasSeleccionadas.length !== 3) {
      this.mensajeError = 'Debes seleccionar exactamente 3 materias.';
      this.cdr.detectChanges();
      return;
    }

    const payload = {
      nombre: this.nombreEstudiante.trim(),
      correo: this.correoEstudiante.trim(),
      materiaIds: this.materiasSeleccionadas
    };

    if (this.editandoId) {
      
      this.abrirConfirmacion(
        'Confirmar Actualización',
        `¿Estás seguro de actualizar los datos de ${this.nombreEstudiante}?`,
        () => {
          this.apiService.actualizarEstudiante(this.editandoId!, payload).subscribe({
            next: (res: any) => {
              this.cancelarEdicion();
              this.cargarDatos();
            },
            error: (err) => {
              this.mensajeError = err.error?.mensaje || 'Ocurrió un error al actualizar.';
              this.cdr.detectChanges();
            }
          });
        }
      );
    } else {
      
      this.abrirConfirmacion(
        'Confirmar Registro',
        `¿Estás seguro de registrar a ${this.nombreEstudiante} con estas 3 materias?`,
        () => {
          this.apiService.registrarEstudiante(payload).subscribe({
            next: (res: any) => {
              this.nombreEstudiante = '';
              this.correoEstudiante = '';
              this.materiasSeleccionadas = [];
              this.cargarDatos();
            },
            error: (err) => {
              this.mensajeError = err.error?.mensaje || 'Ocurrió un error al registrar.';
              this.cdr.detectChanges();
            }
          });
        }
      );
    }
  }


editandoId: number | null = null;

// Preparar formulario para edición
  prepararEdicion(est: any) {
    this.editandoId = est.id;
    this.nombreEstudiante = est.nombre;
    this.correoEstudiante = est.correo;
    this.materiasSeleccionadas = est.materias.map((m: any) => m.id);
    
    this.cdr.detectChanges();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // Cancelar edición
  cancelarEdicion() {
    this.editandoId = null;
    this.nombreEstudiante = '';
    this.correoEstudiante = '';
    this.materiasSeleccionadas = [];
    this.cdr.detectChanges();
  }

  // Variables para el Popup de Confirmación
  mostrarPopup: boolean = false;
  tituloPopup: string = '';
  mensajePopup: string = '';
  accionPendiente: (() => void) | null = null;

  // Método para abrir el popup de confirmación antes de ejecutar algo
  abrirConfirmacion(titulo: string, mensaje: string, accion: () => void) {
    this.tituloPopup = titulo;
    this.mensajePopup = mensaje;
    this.accionPendiente = accion;
    this.mostrarPopup = true;
    this.cdr.detectChanges();
  }

  // Cuando el usuario hace clic en "Sí, estoy seguro"
  ejecutarAccionPopup() {
    if (this.accionPendiente) {
      this.accionPendiente();
    }
    this.cerrarPopup();
  }

  // Cerrar / Cancelar popup
  cerrarPopup() {
    this.mostrarPopup = false;
    this.accionPendiente = null;
    this.cdr.detectChanges();
  }

  // Variables para el Modal de Compañeros
  mostrarModalCompanieros: boolean = false;
  materiaSeleccionadaTitulo: string = '';
  profesorSeleccionadoNombre: string = '';
  listaCompanierosModal: any[] = [];

  // Método actualizado que recibe la materia, el profesor y la lista de compañeros
  verCompanieros(nombreMateria: string, nombreProfesor: string, companieros: any[]) {
    this.materiaSeleccionadaTitulo = nombreMateria;
    this.profesorSeleccionadoNombre = nombreProfesor;
    this.listaCompanierosModal = companieros;
    this.mostrarModalCompanieros = true;
    this.cdr.detectChanges();
  }

  cerrarModalCompanieros() {
    this.mostrarModalCompanieros = false;
    this.listaCompanierosModal = [];
    this.cdr.detectChanges();
  }



  eliminar(id: number) {
    // Confirmación para Eliminar (adiós al confirm() del navegador)
    this.abrirConfirmacion(
      'Confirmar Eliminación',
      '¿Estás seguro de eliminar este estudiante del sistema?',
      () => {
        this.apiService.eliminarEstudiante(id).subscribe({
          next: () => {
            this.cargarDatos();
          },
          error: (err) => {
            console.error('Error al eliminar', err);
            this.mensajeError = 'No se pudo eliminar el registro.';
            this.cdr.detectChanges();
          }
        });
      }
    );
  }
}