using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data;
using Catalog;

namespace Catalog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        CatalogEntities ctx = new CatalogEntities();
        CollectionViewSource studentViewSource;
        CollectionViewSource studentMateriiViewSource;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource studentViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("studentViewSource")));
            this.studentViewSource.Source = ctx.Studentis.Local;
            studentMateriiViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("studentMateriiViewSource")));
            studentMateriiViewSource.Source = ctx.Materiis.Local;
            ctx.Studentis.Load();
            ctx.Materiis.Load();
            ctx.Profesoris.Load();

            cmbStudenti.ItemsSource = ctx.Studentis.Local;
            //cmbStudenti.DisplayMemberPath = "Nume";
            cmbStudenti.SelectedValuePath = "Nrmatricol";

            cmbProfesori.ItemsSource = ctx.Profesoris.Local;
            //cmbProfesori.DisplayMemberPath = " Numeprof";
            cmbProfesori.SelectedValuePath = "idp";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Studenti student = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    student = new Studenti()
                    {
                        Nrmatricol = int.Parse(nrmatricolTextBox.Text.Trim()),
                        Nume = numeTextBox.Text.Trim(),
                        Prenume = prenumeTextBox.Text.Trim(),
                        Materie = materieTextBox.Text.Trim(),
                        Nota = int.Parse(notaTextBox.Text.Trim()),

                        email = emailTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Studentis.Add(student);
                    studentViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {

                if (action == ActionState.Edit)
                {

                    try
                    {
                        student = (Studenti)studentiDataGrid.SelectedItem;
                        student.Nume = numeTextBox.Text.Trim();
                        student.Prenume = prenumeTextBox.Text.Trim();
                        student.Materie = materieTextBox.Text.Trim();
                        student.email = emailTextBox.Text.Trim();
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    studentViewSource.View.Refresh();
                    // pozitionarea pe item-ul curent
                    studentViewSource.View.MoveCurrentTo(student);
                }
                else if (action == ActionState.Delete)
                {
                    try
                    {

                        student = (Studenti)studentiDataGrid.SelectedItem;
                        ctx.Studentis.Remove(student);

                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    studentViewSource.View.Refresh();
                }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            studentViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            studentViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnsave_Click_1(object sender, RoutedEventArgs e)
        {
            Materii mat = null;
            if (action == ActionState.New)
            {
                try
                {
                    Studenti student = (Studenti)cmbStudenti.SelectedItem;
                    Profesori profesor = (Profesori)cmbProfesori.SelectedItem;

                    mat = new Materii()
                    {
                        idp = profesor.idp,
                        Nrmatricol = student.Nrmatricol,
                        Nrcredite = mat.Nrcredite,
                        Numematerie = mat.Numematerie

                    };
                    ctx.Materiis.Add(mat);
                    studentMateriiViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {

                if (action == ActionState.Edit)
                {
                    dynamic Materiaselectata = materiisDataGrid.SelectedItem;
                    try
                    {
                        string curr_id = Materiaselectata.NumeMaterie;
                        var editedmat = ctx.Materiis.FirstOrDefault(s => s.Numematerie == curr_id);
                        if (editedmat != null)
                        {
                            editedmat.Numematerie = cmbStudenti.SelectedValue.ToString();
                            editedmat.idp = Int32.Parse(cmbProfesori.SelectedValue.ToString());
                            //salvam modificarile
                            ctx.SaveChanges();
                        }
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    studentViewSource.View.Refresh();
                    // pozitionarea pe item-ul curent
                    studentViewSource.View.MoveCurrentTo(Materiaselectata);
                }
                else if (action == ActionState.Delete)
                {
                    try
                    {
                        dynamic Materiaselectata = materiisDataGrid.SelectedItem;
                        string curr_id = Materiaselectata.Numematerie;
                        var deletedmat = ctx.Materiis.FirstOrDefault(s => s.Numematerie == curr_id);
                        if (deletedmat != null)
                        {
                            ctx.Materiis.Remove(deletedmat);
                            ctx.SaveChanges();
                            MessageBox.Show("Order Deleted Successfully", "Message");

                        }
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
       
    }
}
