import './CategoryGrid.css';
import { Link } from 'react-router-dom';

const categories = [
  {
    id: 1,
    title: 'Clothes',
    description: 'Explore affordable and sustainable clothing options.',
    image: require('../assets/clothes.jpg'),
    button: 'Shop now',
    link: '/category/clothes'
  },
  {
    id: 2,
    title: 'Accessories',
    description: 'From jewelry to bags, find unique second-hand pieces.',
    image: require('../assets/accessories.jpg'),
    button: 'Learn more',
    link: '/category/accessories'
  },
  {
    id: 3,
    title: 'Toys',
    description: 'Toys with stories — joy for kids and savings for parents.',
    image: require('../assets/toys.jpg'),
    button: 'Learn more',
    link: '/category/toys'
  },
  {
    id: 4,
    title: 'Kids',
    description: 'Clothing and gear your children will outgrow in style.',
    image: require('../assets/kids.jpg'),
    button: 'Learn more',
    link: '/category/kids'
  },
  {
    id: 5,
    title: 'Women’s section',
    description: 'Style meets sustainability — fashion with a story.',
    image: require('../assets/women.jpg'),
    button: 'Learn more',
    link: '/category/women'
  },
  {
    id: 6,
    title: 'Men section',
    description: 'Minimalist, vintage, or rugged looks — second-hand for men.',
    image: require('../assets/men.jpg'),
    button: 'Learn more',
    link: '/category/men'
  },
];

function CategoryGrid() {
  return (
    <section className="category-section">
      <h2 className="category-title">Categories</h2>
      <div className="category-grid">
        {categories.map((cat) => (
          <div className="category-card" key={cat.id}>
            <img src={cat.image} alt={cat.title} />
            <h3>{cat.title}</h3>
            <p>{cat.description}</p>
            <Link to={cat.link}>
              <button>{cat.button}</button>
            </Link>
          </div>
        ))}
      </div>
    </section>
  );
}

export default CategoryGrid;
