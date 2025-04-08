import './GalleryShowcase.css';
import img1 from '../assets/clothes.jpg';
import img2 from '../assets/accessories.jpg';
import img3 from '../assets/toys.jpg';
import img4 from '../assets/women.jpg';


function GalleryShowcase() {
  return (
    <section className="gallery-section">
      <div className="gallery-header">
        <h3>Check us on GitHub</h3>
        <button>Follow us on GitHub</button>
      </div>
      <div className="gallery-images">
        <img src={img1} alt="showcase 1" />
        <img src={img2} alt="showcase 2" />
        <img src={img3} alt="showcase 3" />
        <img src={img4} alt="showcase 4" />
      </div>
    </section>
  );
}

export default GalleryShowcase;
