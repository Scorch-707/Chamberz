using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.Class;

namespace WindowsFormsApplication1
{
    public partial class LightTest : Form
    {
        List<PointF[,]> segment = new List<PointF[,]>();

        //ray data
        List<PointF> points = new List<PointF>();
        List<PointF> uniquePoints = new List<PointF>();
        List<double> uniqueAngles = new List<double>();
        List<PointF[]> rays = new List<PointF[]>();
        List<Tuple<PointF, float>> intersects = new List<Tuple<PointF, float>>();

        public LightTest()
        {
            InitializeComponent();
            loadObstacles();
            this.DoubleBuffered = true;
        }




        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = testPanel.CreateGraphics();
            float radius = 10;
            float x = GlobalVariables.MousePos.X - radius;
            float y = GlobalVariables.MousePos.Y - radius;
            float height = 2 * radius;
            float width = 2 * radius;
            PointF[,] point;
            List<PointF> seg = new List<PointF>();
            double angle;
            Tuple<PointF, float> intersect = new Tuple<PointF, float>(new PointF(), new float());
            Tuple<PointF, float> closestIntersect = new Tuple<PointF, float>(new PointF(), new float());

            
            //draw circle
            g.FillEllipse(Brushes.Aqua, x, y, height, width);

            //reset lists
            points.Clear();
            uniquePoints.Clear();
            uniqueAngles.Clear();
            intersects.Clear();
            rays.Clear();

            for (int ctr = 0; ctr < segment.Count; ctr++)
            {
                point = segment[ctr];
                for (int ctrX = 0; ctrX < point.GetLength(0); ctrX++)
                {
                    //draw segments/polygons 
                    g.DrawLine(Pens.Aqua, point[ctrX, 0], point[ctrX, 1]);

                    for (int ctrY = 0; ctrY < point.GetLength(1); ctrY++)
                    {
                        //get all points
                        points.Add(new PointF(point[ctrX, ctrY].X, point[ctrX, ctrY].Y));

                        if (!(uniquePoints.Contains(point[ctrX, ctrY])))
                        {

                            //get unique points
                            uniquePoints.Add(new PointF(point[ctrX, ctrY].X, point[ctrX, ctrY].Y));

                            //get unique angle
                            angle = Math.Atan2(point[ctrX, ctrY].Y - GlobalVariables.MousePos.Y, point[ctrX, ctrY].X - GlobalVariables.MousePos.X);

                            uniqueAngles.Add(angle - 0.00001);
                            uniqueAngles.Add(angle);
                            uniqueAngles.Add(angle + 0.00001);
                        }
                    }
                }
            }
            
            //Rays in all direction
            int c = 0;
            for (int anglectr = 0; anglectr < uniqueAngles.Count; anglectr++)
            {
                angle = uniqueAngles[anglectr];

                double dx = Math.Cos(angle);
                double dy = Math.Sin(angle);
         
                PointF[] ray = new PointF[2]{
                        new PointF {X = GlobalVariables.MousePos.X, Y = GlobalVariables.MousePos.Y},  
                        new PointF(GlobalVariables.MousePos.X+(float)dx, GlobalVariables.MousePos.Y+(float)dy), 
                };

                rays.Add(ray);

                closestIntersect = null;
                for (int ctr = 0; ctr < segment.Count; ctr++)
                {
                    point = segment[ctr];
                    for (int ctrX = 0; ctrX < point.GetLength(0); ctrX++)
                    {
                        seg.Clear();
                        seg.Add(new PointF(point[ctrX, 0].X, point[ctrX, 0].Y));
                        seg.Add(new PointF(point[ctrX, 1].X, point[ctrX, 1].Y));

                        intersect = getIntersection(ray, seg.ToArray());
                        if (intersect == null) continue;
                        
                        if (closestIntersect == null || intersect.Item2 < closestIntersect.Item2)
                        {
                            closestIntersect = intersect;
                                    
                        }
                        
                    }
                    
                    
                }
                //add to list of intersects
                intersects.Add(closestIntersect);
            } 
  
            //draw rays 
              
            for (int ctr = 0; ctr < intersects.Count; ctr++)
            {
                intersect = intersects[ctr];
                SizeF size = new SizeF(5,5);
                var solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));
 
                RectangleF rec = new RectangleF(intersect.Item1, size);
                g.DrawLine(Pens.Black, new PointF(GlobalVariables.MousePos.X, GlobalVariables.MousePos.Y), intersect.Item1);  
            }
                
            //debug intersects
 
            

        }

        public void loadObstacles()
        {
            PointF[,] Border = new PointF[4, 2] { 
                 { new PointF { X = 0, Y = 0 },  
                   new PointF { X = 640, Y = 0 }
                 }, 

                 { new PointF { X = 640, Y = 0 }, 
                   new PointF { X = 640, Y = 360 }
                 },
 
                 { new PointF { X = 640, Y = 360 },
                   new PointF { X = 0, Y = 360 }
                 },

                 { new PointF { X = 0, Y = 360 },
                   new PointF { X = 0, Y = 0 }
                 },
             };

            PointF[,] Poly1 = new PointF[4, 2] { 
                 { new PointF { X = 100, Y = 150 },  
                   new PointF { X = 120, Y = 50 }
                 }, 

                 { new PointF { X = 120, Y = 50 }, 
                   new PointF { X = 200, Y = 80 }
                 },
 
                 { new PointF { X = 200, Y = 80 },
                   new PointF { X = 140, Y = 210 }
                 },

                 { new PointF { X = 140, Y = 210 },
                   new PointF { X = 100, Y = 150 }
                 },
             };


            PointF[,] Poly2 = new PointF[3, 2]{
                { new PointF { X = 100, Y = 200 },
                   new PointF { X = 120, Y = 250 }
                },

                { new PointF { X = 120, Y = 250 },
                   new PointF { X = 60, Y = 300 }
                },

                { new PointF { X = 60, Y = 300 },
                   new PointF { X = 100, Y = 200 }
                },
            };

            PointF[,] Poly3 = new PointF[4, 2]{
                { new PointF { X = 200, Y = 260 },
                   new PointF { X = 220, Y = 150 }
                },

                { new PointF { X = 220, Y = 150 },
                   new PointF { X = 300, Y = 200 }
                },

                { new PointF { X = 300, Y = 200 },
                   new PointF { X = 350, Y = 320 }
                },

                { new PointF { X = 350, Y = 320 },
                   new PointF { X = 200, Y = 260 }
                },

            };

            PointF[,] Poly4 = new PointF[3, 2]{
                { new PointF { X = 340, Y = 60 },
                   new PointF { X = 360, Y = 40 }
                },

                { new PointF { X = 360, Y = 40 },
                   new PointF { X = 370, Y = 70 }
                },

                { new PointF { X = 370, Y = 70 },
                   new PointF { X = 340, Y = 60 }
                },

            };

            segment.Add(Border);
            segment.Add(Poly1);
            segment.Add(Poly2);
            segment.Add(Poly3);
            segment.Add(Poly4);
        }

        // Find intersection of RAY & SEGMENT
        public Tuple<PointF, float> getIntersection(PointF[] ray, PointF[] segment)
        {
            Tuple<PointF, float> returnset = new Tuple<PointF, float>(new PointF(), new float());
            // RAY in parametric: PointF + Delta*T1
            var r_px = ray[0].X;
            var r_py = ray[0].Y;
            var r_dx = ray[1].X - ray[0].X;
            var r_dy = ray[1].Y - ray[0].Y;

            // SEGMENT in parametric: PointF + Delta*T2
            var s_px = segment[0].X;
            var s_py = segment[0].Y;
            var s_dx = segment[1].X - segment[0].X;
            var s_dy = segment[1].Y - segment[0].Y;

            //Are they parallel? If so, no intersect
            var r_mag = Math.Sqrt(r_dx * r_dx) + (r_dy * r_dy);
            var s_mag = Math.Sqrt(s_dx * s_dx) + (s_dy * s_dy);

            if (r_dx / r_mag == s_dx / s_mag && r_dy / r_mag == s_dy / s_mag)
            {
                return null;
            }

            var T2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
            var T1 = (s_px + s_dx * T2 - r_px) / r_dx;

            if (T1 < 0) return null;
            if (T2 < 0 || T2 > 1) return null;

            var X = r_px + r_dx * T1;
            var Y = r_py + r_dy * T1;

            PointF a = new PointF((float)X, (float)Y);

            //Tuple<PointF, double> returnset = new Tuple<PointF, double>(new PointF((int)Math.Ceiling(X), (int)Math.Ceiling(Y)), T1);
            returnset = new Tuple<PointF, float>(a, T1);

            return returnset;   
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            PointF mouse = PointToClient(MousePosition);
            GlobalVariables.MousePos = new PointF(mouse.X, mouse.Y);
            this.testPanel.Invalidate();
        }
      

    }
}
