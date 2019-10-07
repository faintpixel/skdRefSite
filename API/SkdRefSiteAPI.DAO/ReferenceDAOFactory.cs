using SkdAPI.RefSite.DAO.Models;
using SkdAPI.RefSite.DAO.Models.Animals;
using SkdAPI.RefSite.DAO.Models.People;
using SkdAPI.RefSite.DAO.Models.Structures;
using SkdAPI.RefSite.DAO.Models.Vegetation;
using SkdAPI.RefSite.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdAPI.RefSite.DAO
{
    public class ReferenceDAOFactory
    {
        public static ReferenceDAO<AnimalReference, AnimalClassifications> GetAnimalsDAO()
        {
            return new ReferenceDAO<AnimalReference, AnimalClassifications>(ReferenceType.Animal, new AnimalsQueryable());
        }

        public static ReferenceDAO<FullBodyReference, FullBodyClassifications> GetFullBodiesDAO()
        {
            return new ReferenceDAO<FullBodyReference, FullBodyClassifications>(ReferenceType.FullBody, new FullBodiesQueryable());
        }

        public static ReferenceDAO<BodyPartReference, BodyPartClassifications> GetBodyPartsDAO()
        {
            return new ReferenceDAO<BodyPartReference, BodyPartClassifications>(ReferenceType.BodyPart, new BodyPartsQueryable());
        }

        public static ReferenceDAO<StructureReference, StructureClassifications> GetStructuresDAO()
        {
            return new ReferenceDAO<StructureReference, StructureClassifications>(ReferenceType.Structure, new StructuresQueryable());
        }

        public static ReferenceDAO<VegetationReference, VegetationClassifications> GetVegetationsDAO()
        {
            return new ReferenceDAO<VegetationReference, VegetationClassifications>(ReferenceType.Vegetation, new VegetationQueryable());
        }
    }
}
