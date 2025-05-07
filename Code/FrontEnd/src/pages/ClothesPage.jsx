import ListingGrid from '../components/ListingGrid';
import AnimatedPage from '../components/AnimatedPage';
import axios from 'axios';


function ClothesPage() {
  return (
    <AnimatedPage>
      <ListingGrid category="clothes" />
    </AnimatedPage>
  );
}

export default ClothesPage;
