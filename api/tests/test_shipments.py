import unittest
from httpx import Client
from httpx import codes

# 7 test integrations happy path
# 5 test integrations edge cases

class TestShipments(unittest.TestCase):

    def setUp(self):
        self.client = Client(base_url="http://localhost:3000/api/v1/", headers={"Content-Type": "application/json", "API_KEY": "a1b2c3d4e5"})

    def set_random_key(self):
        self.client.headers["API_KEY"] = "xd"

    def set_reader_key(self):
        self.client.headers["API_KEY"] = "f6g7h8i9j0"

    def test_get_all(self):
        response = self.client.get("/shipments")

        self.assertEqual(response.status_code, codes.OK)
        self.assertTrue(len(response.json()) > 0)

    def test_get_all_unauthorized(self):
        self.set_random_key()
        response = self.client.get("/shipments")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_all_reader(self):
        self.set_reader_key()
        response = self.client.get("/shipments")

        self.assertEqual(response.status_code, codes.OK)
        
    def test_get_single(self):
        response = self.client.get("/shipments/1")

        self.assertEqual(response.status_code, codes.OK)
        self.assertIn("id", response.json())

    def test_get_single_unauthorized(self):
        self.set_random_key()
        response = self.client.get("/shipments/1")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)
    
    def test_get_single_reader(self):
        self.set_reader_key()
        response = self.client.get("/shipments/1")

        self.assertEqual(response.status_code, codes.OK)
    
    def test_create_shipment(self):
        new_shipment = {
            "id": 3,
            "order_id": 55,
            "source_id": 5,
            "order_date": "2024-10-16",
            "request_date": "2024-10-18",
            "shipment_date": "2024-10-20",
            "shipment_type": "I",
            "shipment_status": "Transit",
            "notes": "Snelle levering.",
            "carrier_code": "PostNL",
            "carrier_description": "Royal Dutch Post and Parcel Service",
            "service_code": "TwoDay",
            "payment_type": "Automatic",
            "transfer_mode": "Ground",
            "total_package_count": 12,
            "total_package_weight": 25.5,
            "items": [
                {"item_id": "P003790", "amount": 15},
                {"item_id": "P007369", "amount": 11},
                {"item_id": "P007311", "amount": 25}
            ]
        }
        
        response = self.client.post("/shipments", json=new_shipment)
        self.assertEqual(response.status_code, codes.CREATED)  # HTTP 201 Created

    def test_create_shipment_unauthorized(self):
        self.set_random_key()
        new_shipment = {
            "id": 3,
            "order_id": 55,
            "source_id": 5,
            "order_date": "2024-10-16",
            "request_date": "2024-10-18",
            "shipment_date": "2024-10-20",
            "shipment_type": "I",
            "shipment_status": "Transit",
            "notes": "Snelle levering.",
            "carrier_code": "PostNL",
            "carrier_description": "Royal Dutch Post and Parcel Service",
            "service_code": "TwoDay",
            "payment_type": "Automatic",
            "transfer_mode": "Ground",
            "total_package_count": 12,
            "total_package_weight": 25.5,
            "items": [
                {"item_id": "P003790", "amount": 15},
                {"item_id": "P007369", "amount": 11},
                {"item_id": "P007311", "amount": 25}
            ]
        }
        
        response = self.client.post("/shipments", json=new_shipment)
        self.assertEqual(response.status_code, codes.UNAUTHORIZED)  # HTTP 401 UNAUTHORIZED

    def test_update_shipment(self):
        updated_shipment = {
            "shipment_status": "Delivered",
            "notes": "Package delivered successfully."
        }
        response = self.client.put("/shipments/2", json=updated_shipment)

        self.assertEqual(response.status_code, codes.OK) # HTTP 200 OK
    
    def test_update_shipment_unauthorized(self):
        self.set_random_key()
        
        updated_shipment = {
            "shipment_status": "Delivered",
            "notes": "Package delivered successfully."
        }
        response = self.client.put("/shipments/2", json=updated_shipment)

        self.assertEqual(response.status_code, codes.UNAUTHORIZED) # HTTP 401 UNAUTHORIZED

    def test_delete_shipment(self):
        response = self.client.delete("/shipments/2")

        self.assertEqual(response.status_code, codes.OK)  # HTTP 200 OK

    def test_delete_shipment_unauthorized(self):
        self.set_random_key()
        response = self.client.delete("/shipments/2")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)  # HTTP 401 UNAUTHORIZED



    
