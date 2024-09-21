using AutoMapper;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using SharedClasses.Responses;
namespace BusinessLayer.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //user mapping
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();

            //role mapping
            CreateMap<Role, RoleResponse>();
            CreateMap<CreateRoleRequest, Role>();

            //permission mapping
            CreateMap<Permission, PermissionResponse>();

            //building mapping
            CreateMap<Building, BuildingResponse>();
            CreateMap<BuildingResponse, Building>();
            CreateMap<BuildingRequest, Building>();

            //floor mapping
            CreateMap<Floor, FloorResponse>();
            CreateMap<FloorResponse, Floor>();
            CreateMap<FloorRequest, Floor>();

            //roomType mapping
            CreateMap<RoomType, RoomTypeResponse>();
            CreateMap<RoomTypeResponse, RoomType>();
            CreateMap<RoomTypeRequest, RoomType>();

            //room mapping
            CreateMap<Room,RoomResponse>();
            CreateMap<RoomResponse, Room>();
            CreateMap<RoomRequest, Room>();

            //pharmacy mapping
            CreateMap<Pharmacy, PharmacyResponse>();
            CreateMap<PharmacyResponse, Pharmacy>();
            CreateMap<PharmacyRequest, Pharmacy>();

            //Warehouse mapping
            CreateMap<Warehouse, WarehouseResponse>();
            CreateMap<WarehouseResponse, Warehouse>();
            CreateMap<WarehouseRequest, Warehouse>();

            //Supplier mapping
            CreateMap<Supplier, SupplierResponse>();
            CreateMap<SupplierResponse, Supplier>();
            CreateMap<SupplierRequest, Supplier>();

            //Medicine mapping
            CreateMap<Medicine, MedicineResponse>();
            CreateMap<MedicineResponse, Medicine>();
            CreateMap<MedicineRequest, Medicine>();

            //Section mapping
            CreateMap<Section, SectionResponse>();
            CreateMap<SectionResponse, Section>();
            CreateMap<SectionRequest, Section>();

            //specialization mapping 
            CreateMap<Specialization, SpecializationResponse>();
            CreateMap<SpecializationResponse, Specialization>();
            CreateMap<SpecializationRequest, Specialization>();

            //clinic mapping
            CreateMap<Clinic, ClinicResponse>();
            CreateMap<ClinicResponse, Clinic>();
            CreateMap<ClinicRequest, Clinic>();

            //Nurse mapping
            CreateMap<Nurse, NurseResponse>();
            CreateMap<NurseResponse, Nurse>();
            CreateMap<NurseRequest, Nurse>();

            //Staff mapping
            CreateMap<Staff, StaffResponse>();
            CreateMap<StaffResponse, Staff>();
            CreateMap<StaffRequest, Staff>();

        }

    }
}