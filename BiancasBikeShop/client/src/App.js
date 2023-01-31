import { useEffect, useState } from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import {Navbar, NavbarBrand, NavbarText} from 'reactstrap';
import BikeList from './components/BikeList';
import BikeDetails from './components/BikeDetails';
import { getBikesInShopCount } from './bikeManager';

function App() {
  const [inventory, setInventory] = useState()
  const [detailsBikeId, setDetailsBikeId] = useState(null)

  const getInventory = () => {
    //implement functionality here.... 
    getBikesInShopCount().then(inventory => setInventory(inventory))
  }

  useEffect(() => {
    getInventory()
  }, [])

  return (
    <div>
      <Navbar
      color="light"
      light
      >
        <NavbarBrand href="/">
          <img src="./bike.png" alt="bike" height={50}/>
          <NavbarText style={{marginLeft: '8px'}}>
          Bianca's Bike Shop
        </NavbarText>
        </NavbarBrand>
        <NavbarText>
          Bikes in Garage: {inventory}
        </NavbarText>
      </Navbar>
      <div className='container'>
        <div className='row'>
          <div className="col-sm-8">
            <BikeList setDetailsBikeId={setDetailsBikeId}/>
          </div>
          <div className="col-sm-4">
            <BikeDetails detailsBikeId={detailsBikeId}/>
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
